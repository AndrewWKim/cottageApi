using CottageApi.Core.Configurations;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CottageApi.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, Config config)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = config.Identity.Authority;
                    options.RequireHttpsMetadata = config.Identity.RequireHttpsMetadata;
                });

            return services;
        }
    }
}