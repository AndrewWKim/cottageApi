using System.Net;
using System.Threading.Tasks;
using CottageApi.Core.Configurations;
using CottageApi.Core.Services;
using CottageApi.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CottageApi.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly ICottageDbContext _cottageDbContext;
        private readonly Config _config;

        public TelegramService(
            ICottageDbContext cottageDbContext,
            Config config)
        {
            _config = config;
            _cottageDbContext = cottageDbContext;
        }

        public async Task SendMessageToSecurity(string message)
        {
            var settings = await _cottageDbContext.NewSideSettings.FirstAsync();

            SendMessage(message, settings.TelegramChannelForSecurity);
        }

        private void SendMessage(string message, string chatId)
        {
            var url = $"https://api.telegram.org/bot{_config.TelegramBotApiKey}/sendMessage?chat_id={chatId}&text={message}";

            using (var webClient = new WebClient())
            {
                webClient.DownloadString(url);
            }
        }
    }
}
