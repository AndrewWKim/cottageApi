using CottageApi.Core.Enums;
using CottageApi.Models.Base;

namespace CottageApi.Models.Billing
{
	public class CommunalBillViewModel : BaseModel
	{
		public string BillGUID { get; set; }

		public string CommunalType { get; set; }

		public double Price { get; set; }

		public double MeterData { get; set; }

		public double MeterDataBegin { get; set; }

		public double MeterDataEnd { get; set; }

		public PaymentStatus PaymentStatus { get; set; }
	}
}
