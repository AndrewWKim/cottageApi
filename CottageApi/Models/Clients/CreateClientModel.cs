using System;
using System.Collections.Generic;
using CottageApi.Core.Enums;
using CottageApi.Models.Cars;

namespace CottageApi.Models.Clients
{
	public class CreateClientModel
	{
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ITN { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string AdditionalInfo { get; set; }

        public string PhotoUrl { get; set; }

        public string Passport { get; set; }

        public bool CanVote { get; set; }

        public bool CanPay { get; set; }

        public ClientType ClientType { get; set; }

        public int? ResidentTypeId { get; set; }

        public int CottageId { get; set; }

        public IEnumerable<CarViewModel> Cars { get; set; }
    }
}
