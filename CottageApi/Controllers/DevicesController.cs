using System.Threading.Tasks;
using CottageApi.Controllers.Base;
using CottageApi.Models.Devices;
using CottageApi.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CottageApi.Core.Domain.Entities;

namespace CottageApi.Controllers
{
    [Route("api/[controller]")]
    public class DevicesController : BaseApiController
    {
        private readonly IDeviceService _deviceService;
        private readonly IMapper _mapper;

        public DevicesController(IDeviceService deviceService, IMapper mapper)
        {
            _deviceService = deviceService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<object> CreateDevice(CreateDeviceModel deviceToCreate)
        {
            Device device = _mapper.Map<CreateDeviceModel, Device>(deviceToCreate);
            await _deviceService.CreateDeviceAsync(device);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<object> RemoveClientFromDevice([FromQuery] string deviceId)
        {
            await _deviceService.AssignUserToDeviceAsync(deviceId, null);
            return Ok();
        }
    }
}
