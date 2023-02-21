using System;
using System.Collections.Generic;
using CottageApi.Core.Domain.Entities.Base;

namespace CottageApi.Core.Domain.Entities
{
	public class CottageBilling : BaseEntity
	{
		public DateTime BillingDate { get; set; }

		public int CottageId { get; set; }

		public Cottage Cottage { get; set; }

		public IEnumerable<CommunalBill> CommunalBills { get; set; }
	}
}
