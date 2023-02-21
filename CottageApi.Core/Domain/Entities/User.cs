using CottageApi.Core.Domain.Entities.Base;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Entities
{
	public class User : BaseEntity
	{
		public string Name { get; set; }

		public string Password { get; set; }

		public UserRole Role { get; set; }

		public string BiometricsSignature { get; set; }
	}
}
