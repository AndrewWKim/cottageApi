using System.Collections.Generic;
using CottageApi.Core.Domain.Entities.Base;

namespace CottageApi.Core.Domain.Entities
{
	public class Cottage : BaseEntity
	{
		public string CottageNumber { get; set; }

		public decimal Area { get; set; }

		public int? MainSecurityContactId { get; set; }

		public IEnumerable<Client> Clients { get; set; }
	}
}
