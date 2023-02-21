using CottageApi.Core.Configurations;
using CottageApi.Core.Services;
using OneSignal.RestAPIv3.Client;
using OneSignal.RestAPIv3.Client.Resources;
using OneSignal.RestAPIv3.Client.Resources.Notifications;
using System.Collections.Generic;

namespace CottageApi.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly OneSignalClient _client;
        private readonly Config _config;

        public PushNotificationService(Config config)
        {
            _client = new OneSignalClient(config.OneSignalConfig.APIKey);
            _config = config;
        }

        public void SendPushNotification(List<string> devices, string title, string content)
        {
            var options = CreateOptions(title, content);
            options.IncludePlayerIds = devices;
            _client.Notifications.Create(options);
        }

        private NotificationCreateOptions CreateOptions(string heading, string content)
        {
            var options = new NotificationCreateOptions();
            options.AppId = _config.OneSignalConfig.AppId;
            options.Contents.Add(LanguageCodes.Russian, content);
            options.Headings.Add(LanguageCodes.Russian, heading);
            options.Contents.Add(LanguageCodes.English, content);
            options.Headings.Add(LanguageCodes.English, heading);
            return options;
        }
    }
}
