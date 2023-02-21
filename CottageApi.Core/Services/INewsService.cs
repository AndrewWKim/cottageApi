using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;
using CottageApi.Core.Domain.Dto.News;

namespace CottageApi.Core.Services
{
    public interface INewsService
    {
		Task<Tuple<IEnumerable<CottageNews>, int>> GetNewsAsync(List<NewsStatus> statuses, int? offset, int? limit);

		Task<IEnumerable<ClientNewsViewDto>> GetClientNewsAsync(int userId);

		Task ReadNewsAsync(NewsRead newsRead);

		Task CreateNewsAsync(CottageNews news);

		Task EditNewsAsync(CottageNews newsToEdit);

		Task<CottageNews> GetNewsByIdAsync(int id);
	}
}
