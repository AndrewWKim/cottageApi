using System.Collections.Generic;
using CottageApi.Core.Domain.Entities.Base;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Entities
{
	public class CommunalBill : BaseEntity
	{
		public string BillGUID { get; set; }

		public string BillNumber { get; set; }

		public string CommunalType { get; set; }

		public double Price { get; set; }

		public double MeterData { get; set; }

		public double MeterDataBegin { get; set; }

		public double MeterDataEnd { get; set; }

		public int CottageBillingId { get; set; }

		public CottageBilling CottageBilling { get; set; }

		public PaymentStatus PaymentStatus { get; set; }

		public IEnumerable<PivdenniyPaymentEffort> PivdenniyPaymentEfforts { get; set; }
	}
}
