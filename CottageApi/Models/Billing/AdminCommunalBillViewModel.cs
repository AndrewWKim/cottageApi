using CottageApi.Core.Enums;
using CottageApi.Models.Base;

namespace CottageApi.Models.Billing
{
	public class AdminCommunalBillViewModel : BaseModel
	{
		public string CommunalType { get; set; }

		public string CottageNumber { get; set; }

		public double Price { get; set; }

		public int Month { get; set; }

		public int Year { get; set; }

		public PaymentStatus PaymentStatus { get; set; }

		public string CottageOwner { get; set; }
	}
}
