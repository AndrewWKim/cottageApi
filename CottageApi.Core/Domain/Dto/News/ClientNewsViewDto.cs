using System;
using CottageApi.Core.Domain.Dto.Base;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Domain.Dto.News
{
	public class ClientNewsViewDto : BaseDto
	{
		private NewsType _newsType;

		public DateTime PublicationDate { get; set; }

		public string AdditionalInfo { get; set; }

		public bool IsOpened { get; set; }

		public NewsType NewsType
		{
			get
			{
				return NewsType.CottageNews;
			}

			set
			{
				_newsType = value;
			}
		}
	}
}
