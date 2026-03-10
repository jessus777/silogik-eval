using SilogikEval.Web.Client.Models;

namespace SilogikEval.Web.Client.Services
{
    public interface IApiTranslationService
    {
        Task<IDictionary<string, string>> GetTranslationsAsync(string languageCode);

        Task<IEnumerable<LanguageModel>> GetLanguagesAsync();
    }
}
