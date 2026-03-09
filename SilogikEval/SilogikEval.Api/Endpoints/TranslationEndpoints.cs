using SilogikEval.Application.Dtos;
using SilogikEval.Application.Interfaces;
using SilogikEval.Application.Responses;

namespace SilogikEval.Api.Endpoints
{
    public static class TranslationEndpoints
    {
        public static RouteGroupBuilder MapTranslationEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/translations")
                .WithTags("Translations");

            group.MapGet("/{languageCode}", GetByLanguageAsync)
                .Produces<ApiResponse<IDictionary<string, string>>>();

            group.MapGet("/languages", GetActiveLanguagesAsync)
                .Produces<ApiResponse<IEnumerable<LanguageDto>>>();

            return group;
        }

        private static async Task<IResult> GetByLanguageAsync(
            string languageCode,
            ITranslationServiceAsync translationService)
        {
            var translations = await translationService.GetTranslationsAsync(languageCode);

            return Results.Ok(ApiResponse<IDictionary<string, string>>.Ok(translations));
        }

        private static async Task<IResult> GetActiveLanguagesAsync(
            ITranslationServiceAsync translationService)
        {
            var languages = await translationService.GetActiveLanguagesAsync();

            return Results.Ok(ApiResponse<IEnumerable<LanguageDto>>.Ok(languages));
        }
    }
}
