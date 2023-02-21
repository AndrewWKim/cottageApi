using CottageApi.Models.Base;

namespace CottageApi.Models.Cottages
{
	public class CottageViewModel : BaseModel
	{
		public string CottageNumber { get; set; }

		public decimal Area { get; set; }

		public int MainSecurityContactId { get; set; }
	}
}
