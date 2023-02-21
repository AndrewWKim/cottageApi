using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CottageApi.Core.Domain.Dto;
using CottageApi.Core.Domain.Dto.Ideas;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Services
{
	public interface IIdeaService
	{
		Task<Tuple<IEnumerable<IdeaViewDto>, int>> GetIdeasAsync(List<IdeaStatus> statuses, int? offset, int? limit);

		Task<Tuple<IEnumerable<ClientIdeaViewDto>, int>> GetClientIdeasAsync(int userId, int? offset, int? limit);

		Task<Tuple<IEnumerable<CreatorIdeaViewDto>, int>> GetIdeasByCreatorAsync(int userId, int? offset = null, int? limit = null);

		Task<Idea> CreateIdeaAsync(Idea idea);

		Task VoteIdeaAsync(IdeaVoteDto ideaVoteDto);

		Task ReadIdeaAsync(IdeaRead ideaRead);

		Task EditIdeaAsync(EditIdeaDto ideaToEdit);

		Task<IdeaViewDto> GetIdeaByIdAsync(int id);
	}
}
