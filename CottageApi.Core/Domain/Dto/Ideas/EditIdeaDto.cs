using CottageApi.Core.Domain.Dto.Base;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Dto.Ideas
{
	public class EditIdeaDto : BaseDto
	{
		public string AdditionalInfo { get; set; }

		public IdeaStatus Status { get; set; }
	}
}
