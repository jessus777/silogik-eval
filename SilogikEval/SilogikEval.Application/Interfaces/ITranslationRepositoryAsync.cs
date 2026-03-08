using SilogikEval.Application.Entities;

namespace SilogikEval.Application.Interfaces
{
    public interface ITranslationRepositoryAsync
    {
        Task<IEnumerable<Translation>> GetByLanguageCodeAsync(string languageCode);

        Task<IEnumerable<Language>> GetActiveLanguagesAsync();
    }
}
