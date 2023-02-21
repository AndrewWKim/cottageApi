using System;
using System.Collections.Generic;
using CottageApi.Core.Enums;
using CottageApi.Models.Base;
using CottageApi.Models.Cards;
using CottageApi.Models.Cars;
using CottageApi.Models.Cottages;

namespace CottageApi.Models.Clients
{
	public class ClientViewModel : BaseModel
	{
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ITN { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string AdditionalInfo { get; set; }

        public string PhotoUrl { get; set; }

        public string Passport { get; set; }

        public ClientType ClientType { get; set; }

        public bool CanVote { get; set; }

        public bool CanPay { get; set; }

        public int? ResidentTypeId { get; set; }

        public string RegistrationCode { get; set; }

        public string LoginName { get; set; }

        public string BiometricsSignature { get; set; }

        public CottageViewModel Cottage { get; set; }

        public IEnumerable<CarViewModel> Cars { get; set; }

        public IEnumerable<CardViewModel> Cards { get; set; }
    }
}
