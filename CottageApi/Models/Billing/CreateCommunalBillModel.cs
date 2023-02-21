namespace CottageApi.Models.Billing
{
    public class CreateCommunalBillModel
    {
        public string BillGUID { get; set; }

        public string BillNumber { get; set; }

        public string CommunalType { get; set; }

        public double Price { get; set; }

        public double MeterDataBegin { get; set; }

        public double MeterDataEnd { get; set; }

        public bool IsPaid { get; set; }
    }
}
