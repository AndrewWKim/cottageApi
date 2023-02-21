using System;
using CottageApi.Core.Enums;

namespace CottageApi.Models.News
{
    public class CreateNewsModel
    {
        public DateTime PublicationDate { get; set; }

        public string AdditionalInfo { get; set; }

        public NewsStatus Status { get; set; }
    }
}
