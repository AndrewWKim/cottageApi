using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CottageApi.Controllers.Base;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;
using CottageApi.Core.Services;
using CottageApi.Models.News;
using Microsoft.AspNetCore.Mvc;

namespace CottageApi.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly INewsService _newsService;

        public NewsController(IMapper mapper, INewsService newsService)
        {
            _mapper = mapper;
            _newsService = newsService;
        }

        [HttpGet("{id}")]
        public async Task<object> GetNews([FromRoute] int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            var mappedNews = _mapper.Map<NewsViewModel>(news);
            return Ok(mappedNews);
        }

        [HttpGet]
        public async Task<object> GetAllNews(
            [FromQuery] int? offset = null,
            [FromQuery] int? limit = null,
            [FromQuery] List<NewsStatus> statuses = null)
        {
            var entities = await _newsService.GetNewsAsync(statuses, offset, limit);
            var news = _mapper.Map<List<NewsViewModel>>(entities.Item1);
            return Ok(new
            {
                Total = entities.Item2,
                News = news
            });
        }

        [HttpGet("client")]
        public async Task<object> GetAllNewsForClient()
        {
            var news = await _newsService.GetClientNewsAsync(ClaimsUserId);
            return news;
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> ReadNews([FromRoute] int id)
        {
            var newsRead = new NewsRead
            {
                CottageNewsId = id,
                UserId = ClaimsUserId
            };

            await _newsService.ReadNewsAsync(newsRead);

            return Ok();
        }

        [HttpPost]
        public async Task<object> CreateNews(CreateNewsModel news)
        {
            var newsToCreate = _mapper.Map<CottageNews>(news);
            await _newsService.CreateNewsAsync(newsToCreate);
            return Ok();
        }

        [HttpPut]
        public async Task<object> UpdateNews(EditNewsModel news)
        {
            var newsToEdit = _mapper.Map<CottageNews>(news);
            await _newsService.EditNewsAsync(newsToEdit);
            return Ok();
        }
    }
}
