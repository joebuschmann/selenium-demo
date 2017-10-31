using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace SeleniumAndSpecflow
{
    [Binding]
    public class GoogleSearch
    {
        private IWebDriver _webDriver;
        private IWait<IWebDriver> _defaultWait;
        private string _searchTerm;

        public GoogleSearch(IWebDriver webDriver, IWait<IWebDriver> defaultWait)
        {
            _webDriver = webDriver;
            _defaultWait = defaultWait;
        }

        [Given(@"I navigate to (.*)")]
        public void Navigate(string url)
        {
            if (!url.StartsWith("http") && !url.StartsWith("https"))
                url = "https://" + url;

            _webDriver.Navigate().GoToUrl(url);
        }

        [When(@"I search for (.*)")]
        public void Search(string searchTerm)
        {
            _searchTerm = searchTerm;

            IWebElement searchInput = _webDriver.FindElement(By.CssSelector("input#lst-ib"));
            searchInput.SendKeys(searchTerm);

            IWebElement searchForm = _webDriver.FindElement(By.CssSelector("form[action=\"/search\"]"));
            searchForm.Submit();
        }

        [Then(@"Google should return valid search results")]
        public void ValidateSearchResults()
        {
            IWebElement searchResultsHeader = _defaultWait.Until(d =>
            {
                var results = d.FindElements(By.CssSelector("h2"));
                return results.FirstOrDefault(h => h.GetAttribute("innerText") == "Search Results");
            });

            Assert.IsNotNull(searchResultsHeader);

            IWebElement resultsDiv =
                _defaultWait.Until(
                    ExpectedConditions.ElementExists(By.CssSelector($"div[data-async-context=\"query:{_searchTerm}\"]")));

            Assert.IsNotEmpty(resultsDiv.Text);
        }
    }
}
