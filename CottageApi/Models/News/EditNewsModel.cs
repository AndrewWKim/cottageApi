using CottageApi.Core.Enums;
using CottageApi.Models.Base;

namespace CottageApi.Models.News
{
    public class EditNewsModel : BaseModel
    {
        public string AdditionalInfo { get; set; }

        public NewsStatus Status { get; set; }
    }
}
