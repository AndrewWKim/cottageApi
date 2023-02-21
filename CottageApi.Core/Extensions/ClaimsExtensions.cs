using System;
using System.Linq;
using System.Security.Claims;

namespace CottageApi.Core.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

            if (claim == null)
            {
                throw new Exception($"Can`t get {claimType} from Claim");
            }

            return claim;
        }
    }
}
