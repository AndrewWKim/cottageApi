using System;
using System.Collections.Generic;

namespace CottageApi.Models.Billing
{
    public class CreateCottageBillingModel
    {
        public string ClientItn { get; set; }

        public DateTime BillingDate { get; set; }

        public IEnumerable<CreateCommunalBillModel> CommunalBills { get; set; }
    }
}
