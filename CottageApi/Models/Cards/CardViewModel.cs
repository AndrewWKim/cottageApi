using CottageApi.Models.Base;

namespace CottageApi.Models.Cards
{
    public class CardViewModel : BaseModel
    {
        public string CardPan { get; set; }

        public int ClientId { get; set; }
    }
}
