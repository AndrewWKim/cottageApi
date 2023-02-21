using System.Threading.Tasks;
using AutoMapper;
using CottageApi.Controllers.Base;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CottageApi.Controllers
{
    [Route("api/[controller]")]
    public class NewSideSettingsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly INewSideSettingsService _newSideSettings;

        public NewSideSettingsController(IMapper mapper, INewSideSettingsService newSideSettings)
        {
            _mapper = mapper;
            _newSideSettings = newSideSettings;
        }

        [HttpGet]
        public async Task<object> GetSettings()
        {
            var settings = await _newSideSettings.GetSettingsAsync();
            return settings;
        }

        [HttpGet("rules")]
        public async Task<string> GetCottageRules()
        {
            var rules = await _newSideSettings.GetCottageRulesAsync();
            return rules;
        }

        [HttpPut]
        public async Task<object> UpdateSettings(NewSideSettings settings)
        {
            await _newSideSettings.EditSettingsAsync(settings);
            return Ok();
        }
    }
}
