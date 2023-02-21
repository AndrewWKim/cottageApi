using System.Collections.Generic;

namespace CottageApi.Core.Services
{
    public interface IPushNotificationService
    {
        void SendPushNotification(List<string> devices, string title, string content);
    }
}
