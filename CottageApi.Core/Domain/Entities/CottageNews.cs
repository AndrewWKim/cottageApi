using System;
using System.Collections.Generic;
using CottageApi.Core.Domain.Entities.Base;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Entities
{
	public class CottageNews : BaseEntity
	{
		public DateTime PublicationDate { get; set; }

		public string AdditionalInfo { get; set; }

		public NewsStatus Status { get; set; }

		public IEnumerable<Comment> Comments { get; set; }
	}
}
