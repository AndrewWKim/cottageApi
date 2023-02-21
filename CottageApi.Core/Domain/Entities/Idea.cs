using System;
using System.Collections.Generic;
using CottageApi.Core.Domain.Entities.Base;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Entities
{
	public class Idea : BaseEntity
	{
		public string AdditionalInfo { get; set; }

		public DateTime PublicationDate { get; set; }

		public int VoteCount { get; set; }

		public IEnumerable<Comment> Comments { get; set; }

		public IdeaStatus Status { get; set; }

		public int UserId { get; set; }

		public User User { get; set; }

		public IEnumerable<IdeaVote> IdeaVotes { get; set; }
	}
}
