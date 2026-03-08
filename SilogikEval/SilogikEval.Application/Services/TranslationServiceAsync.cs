using SilogikEval.Application.Dtos;
using SilogikEval.Application.Interfaces;

namespace SilogikEval.Application.Services
{
    public class TranslationServiceAsync
        : ITranslationServiceAsync
    {
        private readonly ITranslationRepositoryAsync _translationRepository;

        public TranslationServiceAsync(ITranslationRepositoryAsync translationRepository)
        {
            _translationRepository = translationRepository;
        }

        public async Task<IDictionary<string, string>> GetTranslationsAsync(string languageCode)
        {
            var translations = await _translationRepository.GetByLanguageCodeAsync(languageCode);

            return translations.ToDictionary(t => t.Key, t => t.Value);
        }

        public async Task<IEnumerable<LanguageDto>> GetActiveLanguagesAsync()
        {
            var languages = await _translationRepository.GetActiveLanguagesAsync();

            return languages.Select(l => new LanguageDto
            {
                Code = l.Code,
                Name = l.Name
            });
        }
    }
}
