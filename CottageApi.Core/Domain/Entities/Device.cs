using CottageApi.Core.Domain.Entities.Base;

namespace CottageApi.Core.Domain.Entities
{
    public class Device : BaseEntity
	{
        public string PlayerId { get; set; }

        public int? ClientId { get; set; }

        public Client Client { get; set; }
    }
}
