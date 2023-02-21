using System;
using System.Collections.Generic;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;

namespace CottageApi.Models.Ideas
{
	public class CreatorIdeaViewModel
	{
		public string AdditionalInfo { get; set; }

		public DateTime PublicationDate { get; set; }

		public int VoteCount { get; set; }

		public IdeaStatus Status { get; set; }

		public IEnumerable<Comment> Comments { get; set; }

		public decimal VotePercentInFavour { get; set; }

		public decimal VotePercentAgainst { get; set; }

		public decimal VotePercentAbstention { get; set; }
	}
}
