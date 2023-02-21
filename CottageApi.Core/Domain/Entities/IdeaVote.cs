using CottageApi.Core.Domain.Entities.Base;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Entities
{
	public class IdeaVote : BaseEntity
	{
		public int IdeaId { get; set; }

		public Idea Idea { get; set; }

		public int CottageId { get; set; }

		public Cottage Cottage { get; set; }

		public VoteType VoteType { get; set; }
	}
}
