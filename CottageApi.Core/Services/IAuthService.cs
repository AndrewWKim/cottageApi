using System.Threading.Tasks;

namespace CottageApi.Core.Services
{
    public interface IAuthService
    {
        Task ResetPassword(string registrationCode, string password);
    }
}
