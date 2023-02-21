using System;
using CottageApi.Core.Enums;

namespace CottageApi.Models.Ideas
{
	public class CreateIdeaModel
	{
		public string AdditionalInfo { get; set; }

		public DateTime PublicationDate { get; set; }

		public int UserId { get; set; }

		public IdeaStatus Status { get; set; }
	}
}
