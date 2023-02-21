namespace CottageApi.Models.Billing
{
    public class PayedCommunalBillsViewModel
    {
		public string BillGUID { get; set; }

		public string BillNumber { get; set; }

		public string CommunalType { get; set; }

		public double Price { get; set; }
	}
}
