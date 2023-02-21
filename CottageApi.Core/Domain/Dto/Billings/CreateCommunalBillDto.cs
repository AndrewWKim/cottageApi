using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Dto.Billings
{
    public class CreateCommunalBillDto
    {
        public string BillGUID { get; set; }

        public string BillNumber { get; set; }

        public string CommunalType { get; set; }

        public double Price { get; set; }

        public double MeterData { get; set; }

        public double MeterDataBegin { get; set; }

        public double MeterDataEnd { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
