using SilogikEval.Api.Endpoints;

namespace SilogikEval.Api.Extensions
{
    public static class EndpointExtensions
    {
        public static WebApplication MapApiEndpoints(this WebApplication app)
        {
            app.MapContactEndpoints();
            app.MapTranslationEndpoints();

            return app;
        }
    }
}
