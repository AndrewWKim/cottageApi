using System;
using CottageApi.Core.Domain.Entities.Base;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Entities
{
	public class PassRequest : BaseEntity
	{
		public PassRequestType PassRequestType { get; set; }

		public string CarBrand { get; set; }

		public string CarModel { get; set; }

		public string CarLicensePlate { get; set; }

		public string VisitorName { get; set; }

		public DateTime VisitDate { get; set; }

		public VisitTime VisitTime { get; set; }

		public string AdditionalInfo { get; set; }

		public int ClientId { get; set; }

		public Client Client { get; set; }
	}
}
