using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Dto.Billings
{
	public class PaymentDataDto
	{
		public int OrderId { get; set; }

		public string HtmlPaymentForm { get; set; }

		public PaymentResultStatus PaymentResultStatus { get; set; }
	}
}
