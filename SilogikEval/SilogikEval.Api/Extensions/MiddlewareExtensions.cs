using SilogikEval.Api.Middleware;

namespace SilogikEval.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorLocalization(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorLocalizationMiddleware>();
        }
    }
}
