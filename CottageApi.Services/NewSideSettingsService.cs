using AutoMapper;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Services;
using CottageApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CottageApi.Services
{
    public class NewSideSettingsService : INewSideSettingsService
    {
        private readonly IMapper _mapper;
        private readonly ICottageDbContext _cottageDbContext;

        public NewSideSettingsService(
            IMapper mapper,
            ICottageDbContext cottageDbContext)
        {
            _mapper = mapper;
            _cottageDbContext = cottageDbContext;
        }

        public async Task EditSettingsAsync(NewSideSettings newNewSideSettings)
        {
            var settings = await _cottageDbContext.NewSideSettings.FirstOrDefaultAsync(n => n.Id == newNewSideSettings.Id);
            settings.SecurityPhoneNumber = newNewSideSettings.SecurityPhoneNumber;
            settings.CottageRulesHTML = newNewSideSettings.CottageRulesHTML;
            settings.TelegramChannelForSecurity = newNewSideSettings.TelegramChannelForSecurity;
            _cottageDbContext.NewSideSettings.Update(settings);
            await _cottageDbContext.SaveChangesAsync();
        }

        public async Task<NewSideSettings> GetSettingsAsync()
        {
            var settings = await _cottageDbContext.NewSideSettings.FirstAsync();

            return settings;
        }

        public async Task<string> GetCottageRulesAsync()
        {
            var settings = await _cottageDbContext.NewSideSettings.FirstAsync();

            return settings.CottageRulesHTML;
        }

    }
}
