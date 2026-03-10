namespace SilogikEval.Web.Client.Services
{
    public class LanguageStateService
        : ILanguageStateService
    {
        public string CurrentLanguage { get; private set; } = "es";

        public event Action? OnLanguageChanged;

        public void SetLanguage(string languageCode)
        {
            if (CurrentLanguage == languageCode)
                return;

            CurrentLanguage = languageCode;
            OnLanguageChanged?.Invoke();
        }
    }
}
