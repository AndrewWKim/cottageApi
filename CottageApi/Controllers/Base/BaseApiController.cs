using System;
using CottageApi.Core.Enums;
using CottageApi.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CottageApi.Controllers.Base
{
    [Authorize]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected int ClaimsUserId => int.Parse(User.GetClaim("id"));

        protected UserRole ClaimsUserRole => (UserRole)Enum.Parse(typeof(UserRole), User.GetClaim("role"));
    }
}