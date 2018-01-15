namespace SeleniumAndSpecflow
{
    public interface ISearchProvider
    {
        void Search(string searchTerm);
        void ValidateConversionResult(string destAmount, string destUnit);
        void ValidateConversionWidgetIsVisible(string type);
        void ValidateDefinition(string word);
        void ValidateDictionaryWidgetIsVisible();
        void ValidateSearchResults();
    }
}