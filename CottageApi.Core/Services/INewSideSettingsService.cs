using System.Threading.Tasks;
using CottageApi.Core.Domain.Entities;

namespace CottageApi.Core.Services
{
    public interface INewSideSettingsService
	{
		Task<NewSideSettings> GetSettingsAsync();

		Task EditSettingsAsync(NewSideSettings newNewSideSettings);

		Task<string> GetCottageRulesAsync();
    }
}
