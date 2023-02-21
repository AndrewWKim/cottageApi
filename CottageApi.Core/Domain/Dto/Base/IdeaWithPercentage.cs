using System.Collections.Generic;
using CottageApi.Core.Domain.Entities;

namespace CottageApi.Core.Domain.Dto.Base
{
	public abstract class IdeaWithPercentage
	{
		public int Id { get; set; }

		public decimal VotePercentInFavour { get; set; }

		public decimal VotePercentAgainst { get; set; }

		public decimal VotePercentAbstention { get; set; }

		public IEnumerable<IdeaVote> IdeaVotes { get; set; }
	}
}
