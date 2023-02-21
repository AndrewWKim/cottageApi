using System;
using CottageApi.Core.Enums;
using CottageApi.Models.Base;

namespace CottageApi.Models.PassRequests
{
	public class PassRequestMobileViewModel : BaseModel
	{
		public PassRequestType PassRequestType { get; set; }

		public string CarBrand { get; set; }

		public string CarModel { get; set; }

		public string CarLicensePlate { get; set; }

		public string VisitorName { get; set; }

		public DateTime VisitDate { get; set; }

		public string VisitTime { get; set; }

		public string AdditionalInfo { get; set; }
	}
}
