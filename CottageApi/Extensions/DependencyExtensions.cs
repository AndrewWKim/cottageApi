using CottageApi.Core.Configurations;
using CottageApi.Core.Services;
using CottageApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CottageApi.Extensions
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddOwnDependencies(this IServiceCollection services, Config config)
        {
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ICottageService, CottageService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IIdeaService, IdeaService>();
            services.AddScoped<IBillingService, BillingService>();
            services.AddScoped<IPassRequestService, PassRequestService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<INewSideSettingsService, NewSideSettingsService>();
            services.AddScoped<IBankPivdenniyService, BankPivdenniyService>();
            services.AddScoped<ITelegramService, TelegramService>();

            return services;
		}
	}
}