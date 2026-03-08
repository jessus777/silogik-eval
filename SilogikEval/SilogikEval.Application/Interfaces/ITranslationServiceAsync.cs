using SilogikEval.Application.Dtos;

namespace SilogikEval.Application.Interfaces
{
    public interface ITranslationServiceAsync
    {
        Task<IDictionary<string, string>> GetTranslationsAsync(string languageCode);

        Task<IEnumerable<LanguageDto>> GetActiveLanguagesAsync();
    }
}
