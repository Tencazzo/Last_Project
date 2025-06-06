namespace Project3.Services
{
    public interface ILocalizationService
    {
        string GetString(string key);
        void SetLanguage(string language);
        string CurrentLanguage { get; }
    }
}
