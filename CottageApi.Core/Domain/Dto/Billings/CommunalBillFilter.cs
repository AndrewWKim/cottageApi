namespace CottageApi.Core.Domain.Dto.Billings
{
	public class CommunalBillFilter
	{
		public int? Offset { get; set; }

		public int? Limit { get; set; }

		public int? CottageId { get; set; }

		public int? Month { get; set; }

		public int? Year { get; set; }
	}
}
