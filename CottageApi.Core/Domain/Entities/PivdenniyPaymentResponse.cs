using CottageApi.Core.Domain.Entities.Base;
using Newtonsoft.Json;

namespace CottageApi.Core.Domain.Entities
{
	public class PivdenniyPaymentResponse : BaseEntity
	{
		public string PurchaseTime { get; set; }

		public string ProxyPan { get; set; }

		public string Currency { get; set; }

		public string ApprovalCode { get; set; }

		public string MerchantID { get; set; }

		public int OrderID { get; set; }

		public string Signature { get; set; }

		public string Rrn { get; set; }

		public string XID { get; set; }

		public string Email { get; set; }

		public string SD { get; set; }

		public string TranCode { get; set; }

		public string TerminalID { get; set; }

		public string TotalAmount { get; set; }

		public string Delay { get; set; }

		public string UPCToken { get; set; }

		public string UPCTokenExp { get; set; }

		public string Comment { get; set; }

		public string HostCode { get; set; }

		[JsonProperty(PropertyName = "Response.Action")]
		public string ResponseAction { get; set; }
	}
}
