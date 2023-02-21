using CottageApi.Core.Domain.Entities.Base;

namespace CottageApi.Core.Domain.Entities
{
	public class NewSideSettings : BaseEntity
	{
		public string SecurityPhoneNumber { get; set; }

		public string CottageRulesHTML { get; set; }

		public string TelegramChannelForSecurity { get; set; }
	}
}
