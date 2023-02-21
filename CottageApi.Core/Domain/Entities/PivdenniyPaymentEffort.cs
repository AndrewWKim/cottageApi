using CottageApi.Core.Domain.Entities.Base;

namespace CottageApi.Core.Domain.Entities
{
	public class PivdenniyPaymentEffort : BaseEntity
	{
		public int OrderId { get; set; }

		public int ClientId { get; set; }

		public int? CommunalBillId { get; set; }

		public CommunalBill CommunalBill { get; set; }
	}
}
