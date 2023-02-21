using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Dto.Billings
{
	public class PivdenniyOrderId
	{
		public string PayCount { get; set; }

		public int ClientId { get; set; }

		public PaymentType PaymentType { get; set; }

		public int BillingId { get; set; }
	}
}
