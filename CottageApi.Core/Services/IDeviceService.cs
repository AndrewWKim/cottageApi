using System.Threading.Tasks;
using CottageApi.Core.Domain.Entities;

namespace CottageApi.Core.Services
{
    public interface IDeviceService
    {
        Task CreateDeviceAsync(Device device);

        Task AssignUserToDeviceAsync(string playerId, int? clientId);
    }
}
