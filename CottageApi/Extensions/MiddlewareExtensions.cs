using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CottageApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace CottageApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGeneralExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GeneralExceptionMiddleware>();
        }

        public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder builder)
        {
            var requestOpt = new RequestLocalizationOptions();
            requestOpt.SupportedCultures = new List<CultureInfo>
            {
                new CultureInfo("ru-RU")
            };

            requestOpt.SupportedUICultures = new List<CultureInfo>
            {
                new CultureInfo("ru-RU")
            };

            requestOpt.RequestCultureProviders.Clear();
            requestOpt.RequestCultureProviders.Add(new SingleCultureProvider());

            return builder.UseRequestLocalization(requestOpt);
        }

        public class SingleCultureProvider : IRequestCultureProvider
        {
            public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
            {
                return Task.Run(() => new ProviderCultureResult("ru-RU", "ru-RU"));
            }
        }
    }
}