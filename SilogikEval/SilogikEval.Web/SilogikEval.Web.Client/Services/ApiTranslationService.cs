using System.Net.Http.Json;
using SilogikEval.Web.Client.Models;

namespace SilogikEval.Web.Client.Services
{
    public class ApiTranslationService
        : IApiTranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, IDictionary<string, string>> _translationsCache = new();
        private IEnumerable<LanguageModel>? _languagesCache;

        public ApiTranslationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IDictionary<string, string>> GetTranslationsAsync(string languageCode)
        {
            if (_translationsCache.TryGetValue(languageCode, out var cached))
                return cached;

            try
            {
                var response = await _httpClient
                    .GetFromJsonAsync<ApiResponseModel<Dictionary<string, string>>>($"api/translations/{languageCode}");

                var translations = response?.Data ?? new Dictionary<string, string>();
                _translationsCache[languageCode] = translations;

                return translations;
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }

        public async Task<IEnumerable<LanguageModel>> GetLanguagesAsync()
        {
            if (_languagesCache is not null)
                return _languagesCache;

            try
            {
                var response = await _httpClient
                    .GetFromJsonAsync<ApiResponseModel<IEnumerable<LanguageModel>>>("api/translations/languages");

                _languagesCache = response?.Data ?? [];

                return _languagesCache;
            }
            catch
            {
                return [];
            }
        }
    }
}
