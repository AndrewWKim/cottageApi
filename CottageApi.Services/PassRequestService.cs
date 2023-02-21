using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Services;
using CottageApi.Data.Context;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CottageApi.Core.Enums;
using CottageApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CottageApi.Services
{
	public class PassRequestService : IPassRequestService
	{
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		private readonly ICottageDbContext _cottageDbContext;
        private readonly ITelegramService _telegramService;

		public PassRequestService(
			IMapper mapper,
			ILogger<PassRequestService> logger,
			ICottageDbContext cottageDbContext,
            ITelegramService telegramService)
		{
			_mapper = mapper;
			_logger = logger;
			_cottageDbContext = cottageDbContext;
            _telegramService = telegramService;
        }

		public async Task<Tuple<IEnumerable<PassRequest>, int>> GetPassRequests(DateTime? visitDate = null, int? cottageId = null, int? offset = null, int? limit = null)
		{
			IQueryable<PassRequest> query = _cottageDbContext.PassRequests
				.Include(p => p.Client)
				.ThenInclude(c => c.Cottage);

			if (cottageId.HasValue)
			{
				query = query.Where(pr => pr.Client.CottageId == cottageId);
			}

			if (visitDate.HasValue)
			{
				query = query.Where(pr => pr.VisitDate.Date == visitDate.Value.Date);
			}

			var total = query.Count();

			var passRequests = await query
				.OrderByDescending(pr => pr.Id)
				.Paging(offset, limit)
				.ToListAsync();

			return new Tuple<IEnumerable<PassRequest>, int>(passRequests, total);
		}

		public async Task<IEnumerable<PassRequest>> GetPassRequestsForMobile(int cottageId)
		{
			IQueryable<PassRequest> query = _cottageDbContext.PassRequests
				.Include(p => p.Client)
				.ThenInclude(c => c.Cottage)
			    .Where(pr => pr.Client.Cottage.Id == cottageId);

			var passRequests = await query
				.OrderByDescending(pr => pr.Id)
				.ToListAsync();

			return passRequests;
		}

		public async Task<PassRequest> CreatePassRequest(PassRequest passRequest)
		{
			_cottageDbContext.PassRequests.Add(passRequest);
			await _cottageDbContext.SaveChangesAsync();

            var timeRussian = passRequest.VisitTime == VisitTime.Morning
                ? "Утром"
                : passRequest.VisitTime == VisitTime.Afternoon
                    ? "Днем"
                    : "Вечером";

            var currentClient = await _cottageDbContext.Clients.Include(c => c.Cottage).AsNoTracking().FirstAsync(client => client.Id == passRequest.ClientId);
			var message = $"Создана новая заявка на пропуск.\n\n  Дата: {passRequest.VisitDate.ToShortDateString()}.\n  Время: {timeRussian}.\n  Имя: {passRequest.VisitorName}.\n  В коттедж номер: №{currentClient.Cottage.CottageNumber}\n";

            if (!string.IsNullOrEmpty(passRequest.AdditionalInfo))
            {
                message += $"\n  Дополнительная информация: {passRequest.AdditionalInfo}\n";

            }

            await _telegramService.SendMessageToSecurity(message);

			return passRequest;
		}

		public async Task DeletePassRequest(int passRequestId)
		{
			var passRequest = await _cottageDbContext.PassRequests.FirstOrDefaultAsync(pr => pr.Id == passRequestId);

			_cottageDbContext.PassRequests.Remove(passRequest);
			await _cottageDbContext.SaveChangesAsync();
		}
	}
}
