using AutoMapper;
using CottageApi.Data.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Services;
using CottageApi.Core.Domain.Dto;
using System.Linq;
using CottageApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using CottageApi.Core.Configurations;
using CottageApi.Core.Domain.Dto.Base;
using CottageApi.Core.Exceptions;
using CottageApi.Core.Domain.Dto.Ideas;
using CottageApi.Core.Enums;

namespace CottageApi.Services
{
    public class IdeaService : IIdeaService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Config _config;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly ICottageDbContext _cottageDbContext;

        public IdeaService(
            IMapper mapper,
            ILogger<IdeaService> logger,
            ICottageDbContext cottageDbContext,
            IPushNotificationService pushNotificationService,
        Config config)
        {
            _mapper = mapper;
            _logger = logger;
            _cottageDbContext = cottageDbContext;
            _config = config;
            _pushNotificationService = pushNotificationService;
        }

        public async Task<Tuple<IEnumerable<IdeaViewDto>, int>> GetIdeasAsync(List<IdeaStatus> statuses, int? offset = null, int? limit = null)
        {
            if (statuses == null || !statuses.Any())
            {
                return new Tuple<IEnumerable<IdeaViewDto>, int>(new List<IdeaViewDto>(), 0);
            }

            var fullArea = _config.CottageVillageArea;

            IQueryable<Idea> query = _cottageDbContext.Ideas.Where(i => statuses.Contains(i.Status)).Include(i => i.IdeaVotes).ThenInclude(iv => iv.Cottage);

            int total = query.Count();

            var ideas = await query
                .OrderByDescending(c => c.PublicationDate)
                .Paging(offset, limit)
                .ToListAsync();

            var ideaViewDtos = _mapper.Map<List<IdeaViewDto>>(ideas);

            foreach (var ideaViewDto in ideaViewDtos)
            {
                CalculateIdeaVotePercents(ideaViewDto, fullArea);
            }

            return new Tuple<IEnumerable<IdeaViewDto>, int>(ideaViewDtos, total);
        }

        public async Task<IdeaViewDto> GetIdeaByIdAsync(int id)
        {
            var idea = await _cottageDbContext.Ideas.FirstOrDefaultAsync(d => d.Id == id);

            if (idea == null)
            {
                throw new NotFoundException();
            }

            var ideaViewDto = _mapper.Map<IdeaViewDto>(idea);

            return ideaViewDto;
        }

        public async Task<Tuple<IEnumerable<ClientIdeaViewDto>, int>> GetClientIdeasAsync(int userId, int? offset = null, int? limit = null)
        {
            var fullArea = _config.CottageVillageArea;

            IQueryable<Idea> query = _cottageDbContext.Ideas.Include(i => i.IdeaVotes).ThenInclude(iv => iv.Cottage).Where(i => i.Status == IdeaStatus.Published);

            int total = query.Count();

            var ideas = await query
                .OrderByDescending(c => c.PublicationDate)
                .Paging(offset, limit)
                .ToListAsync();

            var ideaViews = _mapper.Map<List<ClientIdeaViewDto>>(ideas);

            foreach (var ideaView in ideaViews)
            {
                ideaView.IsOpened = await CheckIsIdeaReaded(userId, ideaView.Id);
                ideaView.IsVoted = await CheckIsVotedIdea(userId, ideaView.Id);
                CalculateIdeaVotePercents(ideaView, fullArea);
            }

            return new Tuple<IEnumerable<ClientIdeaViewDto>, int>(ideaViews, total);
        }

        public async Task<Tuple<IEnumerable<CreatorIdeaViewDto>, int>> GetIdeasByCreatorAsync(int userId, int? offset = null, int? limit = null)
        {
            var fullArea = _config.CottageVillageArea;

            IQueryable<Idea> query = _cottageDbContext.Ideas
                .Include(i => i.IdeaVotes)
                    .ThenInclude(iv => iv.Cottage)
                .Include(i => i.Comments)
                .Where(i => i.UserId == userId);

            int total = query.Count();

            var ideas = await query
                .OrderByDescending(c => c.PublicationDate)
                .Paging(offset, limit)
                .ToListAsync();

            var ideaViewDtos = _mapper.Map<List<CreatorIdeaViewDto>>(ideas);

            foreach (var ideaViewDto in ideaViewDtos)
            {
                CalculateIdeaVotePercents(ideaViewDto, fullArea);
            }

            return new Tuple<IEnumerable<CreatorIdeaViewDto>, int>(ideaViewDtos, total);
        }

        public async Task<Idea> CreateIdeaAsync(Idea idea)
        {
            _cottageDbContext.Ideas.Add(idea);
            await _cottageDbContext.SaveChangesAsync();

            // TODO APP_NOTIFICATION
            if (idea.Status == IdeaStatus.Published)
            {
                var devices = _cottageDbContext.Devices.Where(dev => dev.ClientId.HasValue).Select(dev => dev.PlayerId).ToList();

                if (devices.Any())
                {
                    _pushNotificationService.SendPushNotification(devices, "Опубликована новая идея", idea.AdditionalInfo);
                }

            }
            return idea;
        }

        public async Task VoteIdeaAsync(IdeaVoteDto ideaVoteDto)
        {

            var client = await _cottageDbContext.Clients.AsNoTracking().FirstOrDefaultAsync(cl => cl.UserId == ideaVoteDto.UserId);

            var existingIdeaVote = await _cottageDbContext.IdeaVotes.AsNoTracking().FirstOrDefaultAsync(iv => iv.CottageId == client.CottageId && iv.IdeaId == ideaVoteDto.IdeaId);

            if (existingIdeaVote != null)
            {
                throw new ValidationException("idea", "Вы уже голосовали за эту идею");
            }

            var idea = await _cottageDbContext.Ideas.FirstOrDefaultAsync(i => i.Id == ideaVoteDto.IdeaId);

            idea.VoteCount += 1;

            _cottageDbContext.Ideas.Update(idea);
            await _cottageDbContext.SaveChangesAsync();

            var ideaVote = new IdeaVote()
            {
                IdeaId = idea.Id,
                CottageId = client.CottageId,
                VoteType = ideaVoteDto.VoteType
            };

            await CreateIdeaVote(ideaVote);
        }

        public async Task ReadIdeaAsync(IdeaRead ideaRead)
        {
            _cottageDbContext.IdeaReads.Add(ideaRead);
            await _cottageDbContext.SaveChangesAsync();
        }

        public async Task EditIdeaAsync(EditIdeaDto ideaToEdit)
        {
            var idea = await _cottageDbContext.Ideas.FirstOrDefaultAsync(i => i.Id == ideaToEdit.Id);
            idea.AdditionalInfo = ideaToEdit.AdditionalInfo;
            var prevStatus = idea.Status;
            idea.Status = ideaToEdit.Status;
            idea.PublicationDate = DateTime.Now;

            _cottageDbContext.Ideas.Update(idea);
            await _cottageDbContext.SaveChangesAsync();

            // TODO APP_NOTIFICATION
            if (ideaToEdit.Status == IdeaStatus.Published && prevStatus != IdeaStatus.Published)
            {
                var devices = _cottageDbContext.Devices.Where(dev => dev.ClientId.HasValue).Select(dev => dev.PlayerId).ToList();
                if (devices.Any())
                {
                    _pushNotificationService.SendPushNotification(devices, "Опубликована новая идея", idea.AdditionalInfo);
                }
            }
        }

        private async Task CreateIdeaVote(IdeaVote ideaVote)
        {
            _cottageDbContext.IdeaVotes.Add(ideaVote);
            await _cottageDbContext.SaveChangesAsync();
        }

        private async Task<bool> CheckIsVotedIdea(int userId, int ideaId)
        {
            var client = await _cottageDbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);

            var ideaVote = _cottageDbContext.IdeaVotes
                .FirstOrDefault(iv => iv.IdeaId == ideaId && iv.CottageId == client.CottageId);

            return ideaVote != null;
        }

        private async Task<bool> CheckIsIdeaReaded(int userId, int ideaId)
        {
            var ideaRead = await _cottageDbContext.IdeaReads
                .FirstOrDefaultAsync(ir => ir.IdeaId == ideaId && ir.UserId == userId);

            return ideaRead != null;
        }

        private void CalculateIdeaVotePercents(IdeaWithPercentage idea, decimal fullArea)
        {
            foreach (var ideaVote in idea.IdeaVotes)
            {
                var cottageArea = ideaVote.Cottage.Area;

                // -1 because testing cottage has 1 area
                decimal votePercent = cottageArea * 100 / (fullArea - 1);
                votePercent = decimal.Round(votePercent, 2, MidpointRounding.AwayFromZero);

                switch (ideaVote.VoteType)
                {
                    case VoteType.InFavour:
                        idea.VotePercentInFavour += votePercent;
                        break;
                    case VoteType.Against:
                        idea.VotePercentAgainst += votePercent;
                        break;
                    case VoteType.Abstention:
                        idea.VotePercentAbstention += votePercent;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
