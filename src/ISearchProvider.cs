namespace SeleniumAndSpecflow
{
    public interface ISearchProvider
    {
		void InitializeBrowser();
		void Search(string searchTerm);
        void ValidateConversionResult(string destAmount, string destUnit);
        void ValidateConversionWidgetIsVisible(string type);
        void ValidateDefinition(string definition);
        void ValidateDictionaryWidgetIsVisible();
        void ValidateSearchResults();
    }
}