using CottageApi.Models.Base;

namespace CottageApi.Models.Cars
{
	public class CarViewModel : BaseModel
	{
		public string Brand { get; set; }

		public string Model { get; set; }

		public string CarLicensePlate { get; set; }

		public string ClientFullName { get; set; }
	}
}
