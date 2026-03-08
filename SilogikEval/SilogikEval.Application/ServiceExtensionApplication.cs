using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SilogikEval.Application.Interfaces;
using SilogikEval.Application.Services;
using SilogikEval.Application.Validators;

namespace SilogikEval.Application
{
    public static class ServiceExtensionApplication
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateContactValidator>();
            services.AddScoped<IContactServiceAsync, ContactServiceAsync>();
            services.AddScoped<ITranslationServiceAsync, TranslationServiceAsync>();

            return services;
        }
    }
}
