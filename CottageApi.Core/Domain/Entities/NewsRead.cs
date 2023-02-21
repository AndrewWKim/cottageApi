using CottageApi.Core.Domain.Entities.Base;

namespace CottageApi.Core.Domain.Entities
{
	public class NewsRead : BaseEntity
	{
		public int CottageNewsId { get; set; }

		public CottageNews CottageNews { get; set; }

		public int UserId { get; set; }

		public User User { get; set; }
	}
}
