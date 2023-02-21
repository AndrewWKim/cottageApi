using System.Threading.Tasks;

namespace CottageApi.Core.Services
{
    public interface ITelegramService
    {
        Task SendMessageToSecurity(string message);
    }
}
