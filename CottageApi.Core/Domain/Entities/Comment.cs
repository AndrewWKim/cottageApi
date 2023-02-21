using CottageApi.Core.Domain.Entities.Base;

namespace CottageApi.Core.Domain.Entities
{
	public class Comment : BaseEntity
	{
		public string Text { get; set; }

		public int? CottageNewsId { get; set; }

		public CottageNews CottageNews { get; set; }

		public int? IdeaId { get; set; }

		public Idea Idea { get; set; }

		public int ClientId { get; set; }

		public Client Client { get; set; }
	}
}
