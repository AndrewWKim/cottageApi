using CottageApi.Core.Domain.Entities.Base;

namespace CottageApi.Core.Domain.Entities
{
	public class Card : BaseEntity
	{
		public string CardPan { get; set; }

		public string CardToken { get; set; }

		public string PurchaseTime { get; set; }

		public int ClientId { get; set; }

		public Client Client { get; set; }
	}
}
