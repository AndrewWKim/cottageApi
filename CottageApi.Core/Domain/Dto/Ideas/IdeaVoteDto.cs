using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Dto
{
	public class IdeaVoteDto
	{
		public int UserId { get; set; }

		public int IdeaId { get; set; }

		public VoteType VoteType { get; set; }
	}
}
