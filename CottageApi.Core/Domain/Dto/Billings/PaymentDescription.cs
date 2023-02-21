using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Dto.Billings
{
	public class PaymentDescription
	{
		public string ClientFirstName { get; set; }

		public string ClientLastName { get; set; }

		public PaymentType PaymentType { get; set; }

		public int BillingId { get; set; }

		public string BillingName { get; set; }

		public int Month { get; set; }

		public int Year { get; set; }
	}
}
