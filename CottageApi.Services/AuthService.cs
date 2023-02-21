using CottageApi.Core.Exceptions;
using CottageApi.Core.Helpers;
using CottageApi.Core.Services;
using CottageApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CottageApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICottageDbContext _cottageDbContext;

        public AuthService(ICottageDbContext cottageDbContext)
        {
            _cottageDbContext = cottageDbContext;
        }

        public async Task ResetPassword(string registrationCode, string password)
        {
            string encryptedPassword = EncryptionHelper.HashWithMD5(password);
            var client = await _cottageDbContext.Clients.AsNoTracking().FirstOrDefaultAsync(cl => cl.RegistrationCode == registrationCode);
            if (client == null || client.UserId == null)
            {
                throw new ValidationException("registrationCode", "Пользователь не найден.");
            }
            var user = await _cottageDbContext.Users.FirstOrDefaultAsync(usr => usr.Id == client.UserId);
            if (user.Password == encryptedPassword)
            {
                throw new ValidationException("password", "Старый и новый пароли не могут совпадать.");
            }
            user.Password = encryptedPassword;
            _cottageDbContext.Users.Update(user);
            await _cottageDbContext.SaveChangesAsync();
        }
    }
}
