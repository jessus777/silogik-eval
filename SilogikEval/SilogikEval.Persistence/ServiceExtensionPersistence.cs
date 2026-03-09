using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilogikEval.Application.Interfaces;
using SilogikEval.Persistence.Context;
using SilogikEval.Persistence.Repositories;
using SilogikEval.Persistence.Services;

namespace SilogikEval.Persistence
{
    public static class ServiceExtensionPersistence
    {
        public static IServiceCollection AddPersistenceServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddSingleton<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

            services.AddScoped<IContactRepositoryAsync, ContactRepositoryAsync>();
            services.AddScoped<ITranslationRepositoryAsync, TranslationRepositoryAsync>();

            services.AddScoped<IFileValidator, FileValidator>();

            var uploadPath = configuration["FileStorage:BasePath"]
                ?? Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            services.AddScoped<IFileStorageService>(_ => new LocalFileStorageService(uploadPath));

            return services;
        }
    }
}
