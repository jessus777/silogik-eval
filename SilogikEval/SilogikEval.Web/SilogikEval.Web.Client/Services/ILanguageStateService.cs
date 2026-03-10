namespace SilogikEval.Web.Client.Services
{
    public interface ILanguageStateService
    {
        string CurrentLanguage { get; }

        event Action? OnLanguageChanged;

        void SetLanguage(string languageCode);
    }
}
