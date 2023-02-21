using System;
using CottageApi.Core.Enums;
using CottageApi.Models.Base;

namespace CottageApi.Models.News
{
    public class NewsViewModel : BaseModel
    {
        public DateTime PublicationDate { get; set; }

        public string AdditionalInfo { get; set; }

        public NewsStatus Status { get; set; }
    }
}
