using System;
using CottageApi.Models.Base;

namespace CottageApi.Models.Billing
{
	public class CottageBillingViewModel : BaseModel
	{
		public DateTime BillingDate { get; set; }

		public decimal TotalPrice { get; set; }
	}
}
