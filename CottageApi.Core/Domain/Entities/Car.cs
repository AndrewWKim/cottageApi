using CottageApi.Core.Domain.Entities.Base;

namespace CottageApi.Core.Domain.Entities
{
	public class Car : BaseEntity
	{
		public string Brand { get; set; }

		public string Model { get; set; }

		public string CarLicensePlate { get; set; }

		public int ClientId { get; set; }

		public Client Client { get; set; }
	}
}
