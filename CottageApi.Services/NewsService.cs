using AutoMapper;
using CottageApi.Core.Domain.Dto.News;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;
using CottageApi.Core.Exceptions;
using CottageApi.Core.Extensions;
using CottageApi.Core.Services;
using CottageApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CottageApi.Services
{
    public class NewsService : INewsService
    {
        private readonly IMapper _mapper;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly ICottageDbContext _cottageDbContext;

        public NewsService(
            IMapper mapper,
            ICottageDbContext cottageDbContext,
            IPushNotificationService pushNotificationService)
        {
            _mapper = mapper;
            _cottageDbContext = cottageDbContext;
            _pushNotificationService = pushNotificationService;
        }

        public async Task CreateNewsAsync(CottageNews news)
        {
            var createdNews = _cottageDbContext.CottageNews.Add(news);
            await _cottageDbContext.SaveChangesAsync();

            // TODO APP_NOTIFICATION
            var devices = _cottageDbContext.Devices.Where(dev => dev.ClientId.HasValue).Select(dev => dev.PlayerId).ToList();
            if (devices.Any())
            {
                _pushNotificationService.SendPushNotification(devices, "Опубликована новая новость", news.AdditionalInfo);
            }
        }

        public async Task EditNewsAsync(CottageNews newsToEdit)
        {
            var news = await _cottageDbContext.CottageNews.FirstOrDefaultAsync(n => n.Id == newsToEdit.Id);
            news.AdditionalInfo = newsToEdit.AdditionalInfo;
            news.Status = newsToEdit.Status;
            news.PublicationDate = DateTime.Now;

           var editedNews = _cottageDbContext.CottageNews.Update(news);
            await _cottageDbContext.SaveChangesAsync();

            // TODO APP_NOTIFICATION
            if (newsToEdit.Status == NewsStatus.Published)
            {
                var devices = _cottageDbContext.Devices.Where(dev => dev.ClientId.HasValue).Select(dev => dev.PlayerId).ToList();
                if (devices.Any())
                {
                    _pushNotificationService.SendPushNotification(devices, "Новость была изменена.", news.AdditionalInfo);
                }
            }
        }

        public async Task<Tuple<IEnumerable<CottageNews>, int>> GetNewsAsync(List<NewsStatus> statuses, int? offset, int? limit)
        {
            if (statuses == null || !statuses.Any())
            {
                return new Tuple<IEnumerable<CottageNews>, int>(new List<CottageNews>(), 0);
            }

            IQueryable<CottageNews> query = _cottageDbContext.CottageNews.Where(i => statuses.Contains(i.Status));

            int total = query.Count();

            var news = await query
                .OrderByDescending(c => c.PublicationDate)
                .Paging(offset, limit)
                .ToListAsync();

            return new Tuple<IEnumerable<CottageNews>, int>(news, total);
        }

        public async Task<IEnumerable<ClientNewsViewDto>> GetClientNewsAsync(int userId)
        {
            var news = await _cottageDbContext.CottageNews.Where(cn => cn.Status == NewsStatus.Published)
                .OrderByDescending(c => c.PublicationDate)
                .ToListAsync();

            var newsViews = _mapper.Map<List<ClientNewsViewDto>>(news);

            foreach (var newsView in newsViews)
            {
                newsView.IsOpened = await CheckIsNewsReaded(userId, newsView.Id);
            }

            return newsViews;
        }

        public async Task ReadNewsAsync(NewsRead newsRead)
        {
            _cottageDbContext.NewsReads.Add(newsRead);
            await _cottageDbContext.SaveChangesAsync();
        }

        public async Task<CottageNews> GetNewsByIdAsync(int id)
        {
            var news = await _cottageDbContext.CottageNews.FindByIdAsync(id);

            if (news == null)
            {
                throw new NotFoundException("Новость не найдена.");
            }

            return news;
        }

        private async Task<bool> CheckIsNewsReaded(int userId, int newsId)
        {
            var ideaRead = await _cottageDbContext.NewsReads
                .FirstOrDefaultAsync(cnr => cnr.CottageNewsId == newsId && cnr.UserId == userId);

            return ideaRead != null;
        }
    }
}
