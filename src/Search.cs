using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace SeleniumAndSpecflow
{
    [Binding]
    public class Search
    {
        private readonly IWebDriver _webDriver;
        private readonly ISearchProvider _searchProvider;

        public Search(IWebDriver webDriver, ISearchProvider searchProvider)
        {
            _webDriver = webDriver;
            _searchProvider = searchProvider;
        }

        [Given(@"I navigate to (.*)")]
        public void Navigate(string url)
        {
            if (!url.StartsWith("http") && !url.StartsWith("https"))
                url = "https://" + url;

            _webDriver.Navigate().GoToUrl(url);
        }

        [When(@"I search for (.*)")]
        public void DoSearch(string searchTerm)
        {
            _searchProvider.Search(searchTerm);
        }

        [Then(@"the search engine should return valid search results")]
        public void ValidateSearchResults()
        {
            _searchProvider.ValidateSearchResults();
        }

        [When(@"I convert (.*) (.*) to (.*)")]
        public void Convert(string srcAmount, string srcUnit, string destUnit)
        {
            string searchTerm = $"convert {srcAmount} {srcUnit} to {destUnit}";
            DoSearch(searchTerm);
        }

        [Then(@"the search engine should show the conversion widget for (.*)")]
        public void ValidateConversionWidgetIsVisible(string type)
        {
            _searchProvider.ValidateConversionWidgetIsVisible(type);
        }

        [Then(@"the conversion result should be (.*) (.*)")]
        public void ValidateConversionResult(string destAmount, string destUnit)
        {
            _searchProvider.ValidateConversionResult(destAmount, destUnit);
        }

        [Then(@"the search engine should show the dictionary widget")]
        public void ValidateDictionaryWidgetIsVisible()
        {
            _searchProvider.ValidateDictionaryWidgetIsVisible();
        }

        [Then(@"the definition for (.*) should be displayed")]
        public void ValidateDefinition(string word)
        {
            _searchProvider.ValidateDefinition(word);
        }
    }
}
