using AutoMapper;
using Microsoft.Extensions.Logging;
using CottageApi.Data.Context;
using System.Threading.Tasks;
using CottageApi.Core.Services;
using System.Linq;
using CottageApi.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CottageApi.Core.Exceptions;

namespace CottageApi.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private readonly ICottageDbContext _cottageDbContext;

        public DeviceService(
            IMapper mapper,
            ILogger<IdeaService> logger,
            ICottageDbContext cottageDbContext)
        {
            _mapper = mapper;
            _logger = logger;
            _cottageDbContext = cottageDbContext;
        }

        public async Task CreateDeviceAsync(Device device)
        {
            if (device.ClientId.HasValue)
            {
                device.ClientId = (await _cottageDbContext.Clients.FirstOrDefaultAsync(c => c.UserId == device.ClientId)).Id;
            }

            if (string.IsNullOrEmpty(device.PlayerId))
            {
                throw new ValidationException("PlayerId", "PlayerId не может быть null.");
            }
            if (_cottageDbContext.Devices.Any(d => d.PlayerId == device.PlayerId))
            {
                await AssignUserToDeviceAsync(device.PlayerId, device.ClientId);
                return;
            }
            _cottageDbContext.Devices.Add(device);
            await _cottageDbContext.SaveChangesAsync();
        }

        public async Task AssignUserToDeviceAsync(string playerId, int? clientId)
        {
            var device = await _cottageDbContext.Devices.FirstOrDefaultAsync(d => d.PlayerId == playerId);
            if (device == null)
            {
                throw new ValidationException("PlayerId", "Устройство не найдено");
            }
            device.ClientId = clientId;
            _cottageDbContext.Devices.Update(device);
            await _cottageDbContext.SaveChangesAsync();
        }
    }
}
