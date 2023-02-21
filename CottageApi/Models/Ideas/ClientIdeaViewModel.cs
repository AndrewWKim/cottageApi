using System;
using System.Collections.Generic;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;
using CottageApi.Models.Base;

namespace CottageApi.Models.Ideas
{
	public class ClientIdeaViewModel : BaseModel
	{
		public string AdditionalInfo { get; set; }

		public DateTime PublicationDate { get; set; }

		public int VoteCount { get; set; }

		public IEnumerable<Comment> Comments { get; set; }

		public bool IsVoted { get; set; }

		public bool IsOpened { get; set; }

		public NewsType NewsType { get; set; }

		public decimal VotePercentInFavour { get; set; }

		public decimal VotePercentAgainst { get; set; }

		public decimal VotePercentAbstention { get; set; }
	}
}
