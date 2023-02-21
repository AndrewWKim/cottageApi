using System.Threading.Tasks;
using AutoMapper;
using CottageApi.Controllers.Base;
using CottageApi.Core.Services;
using CottageApi.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CottageApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AuthController(IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<object> ResetPassword(ResetPasswordModel model)
        {
            await _authService.ResetPassword(model.RegistrationCode, model.Password);
            return Ok();
        }
    }
}
