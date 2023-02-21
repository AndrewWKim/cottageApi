using System;
using System.Collections.Generic;

namespace CottageApi.Core.Domain.Dto.Billings
{
    public class CreateCottageBillingDto
    {
        public string ClientItn { get; set; }

        public DateTime BillingDate { get; set; }

        public IEnumerable<CreateCommunalBillDto> CommunalBills { get; set; }
    }
}
