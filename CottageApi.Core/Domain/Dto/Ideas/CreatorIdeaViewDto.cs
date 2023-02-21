using System;
using System.Collections.Generic;
using CottageApi.Core.Domain.Dto.Base;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Dto.Ideas
{
    public class CreatorIdeaViewDto : IdeaWithPercentage
	{
		public string AdditionalInfo { get; set; }

		public DateTime PublicationDate { get; set; }

		public int VoteCount { get; set; }

		public IdeaStatus Status { get; set; }

		public IEnumerable<Comment> Comments { get; set; }
	}
}
