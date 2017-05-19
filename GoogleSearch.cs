using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace SeleniumAndSpecflow
{
    [Binding]
    public class GoogleSearch : IDisposable
    {
        private readonly IWebDriver _webDriver;
        private readonly IWait<IWebDriver> _defaultWait;
        private string _searchTerm;

        public GoogleSearch()
        {
            _webDriver = new ChromeDriver();

            _defaultWait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(5))
            {
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };
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

        [AfterScenario]
        public void Dispose()
        {
            if (_webDriver != null)
            {
                _webDriver.Quit();
            }
        }
    }
}
