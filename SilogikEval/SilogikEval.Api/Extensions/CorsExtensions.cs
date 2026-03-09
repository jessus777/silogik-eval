namespace SilogikEval.Api.Extensions
{
    public static class CorsExtensions
    {
        private const string PolicyName = "AllowFrontend";

        public static IServiceCollection AddAppCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(PolicyName, policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            return app.UseCors(PolicyName);
        }
    }
}
