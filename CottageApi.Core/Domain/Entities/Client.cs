using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CottageApi.Core.Domain.Entities.Base;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Entities
{
    public class Client : BaseEntity
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

        public int? ResidentTypeId { get; set; }

        public ResidentType ResidentType { get; set; }

        public bool CanVote { get; set; }

        public bool CanPay { get; set; }

        public string RegistrationCode { get; set; }

        public int PayCount { get; set; }

        public int CottageId { get; set; }

        public Cottage Cottage { get; set; }

        public int? UserId { get; set; }

        public User User { get; set; }

        public IEnumerable<Car> Cars { get; set; }

        public IEnumerable<Card> Cards { get; set; }

        public virtual string GetFullName()
        {
	        return $"{FirstName} {LastName}";
        }

        public virtual int ParsePhoneToDigit()
        {
            var replacedString = PhoneNumber.Replace(" ", string.Empty).Replace("-", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);
            var resultString = Regex.Match(PhoneNumber, @"\d+").Value;
            return int.Parse(resultString);
        }
    }
}