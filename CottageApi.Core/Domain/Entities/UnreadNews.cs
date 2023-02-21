using CottageApi.Core.Domain.Entities.Base;

namespace CottageApi.Core.Domain.Entities
{
	public class UnreadNews : BaseEntity
	{
		public int CottageNewsId { get; set; }

		public CottageNews CottageNews { get; set; }

		public int ClientId { get; set; }

		public Client Client { get; set; }
	}
}
