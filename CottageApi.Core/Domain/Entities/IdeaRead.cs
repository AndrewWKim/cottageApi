using CottageApi.Core.Domain.Entities.Base;

namespace CottageApi.Core.Domain.Entities
{
	public class IdeaRead : BaseEntity
	{
		public int IdeaId { get; set; }

		public Idea Idea { get; set; }

		public int UserId { get; set; }
	}
}
