using SilogikEval.Web.Client.Services;

namespace SilogikEval.Web.Client.Extensions
{
    public static class ServiceExtensionClient
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services, Uri apiBaseUrl)
        {
            services.AddScoped(sp => new HttpClient { BaseAddress = apiBaseUrl });

            services.AddSingleton<ILanguageStateService, LanguageStateService>();
            services.AddScoped<IApiTranslationService, ApiTranslationService>();
            services.AddScoped<IApiContactService, ApiContactService>();

            return services;
        }
    }
}
