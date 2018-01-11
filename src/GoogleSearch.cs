using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private IWebElement _widgetElement;

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

        [When(@"I convert (.*) (.*) to (.*)")]
        public void Convert(string srcAmount, string srcUnit, string destUnit)
        {
            string searchTerm = $"convert {srcAmount} {srcUnit} to {destUnit}";
            Search(searchTerm);
        }

        [Then(@"Google should show the conversion widget for (.*)")]
        public void ValidateConversionWidgetIsVisible(string type)
        {
            // Find the widget container to verify it exists
            _widgetElement = _defaultWait.Until(d =>
            {
                var results = d.FindElements(By.CssSelector("div.obcontainer"));
                return results.FirstOrDefault();
            });

            Assert.IsNotNull(_widgetElement);

            // Make sure the correct measurement type is selected in the form
            IWebElement selectedOption = _defaultWait.Until(d =>
                _widgetElement.FindElement(By.CssSelector("div._frf select option:checked")));

            Assert.IsNotNull(selectedOption);
            Assert.AreEqual(type.ToLower(), selectedOption.Text.ToLower());
            Assert.AreEqual(type.ToLower(), selectedOption.GetAttribute("value").ToLower());
        }

        [Then(@"the conversion result should be (.*) (.*)")]
        public void ValidateConversionResult(string destAmount, string destUnit)
        {
            // Make sure the correct amount is displayed
            IWebElement amountElement = _defaultWait.Until(d =>
            {
                return _widgetElement.FindElement(By.CssSelector("div#_Cif input"));
            });

            Assert.AreEqual(destAmount, amountElement.GetAttribute("value"));

            // Make sure the correct unit is selected
            IWebElement selectedOption = _defaultWait.Until(d =>
            {
                return _widgetElement.FindElement(By.CssSelector("div#_Cif select option:checked"));
            });

            Assert.IsNotNull(selectedOption);
            Assert.AreEqual(destUnit.ToLower(), selectedOption.Text.ToLower());
        }

        [Then(@"Google should show the dictionary widget")]
        public void ValidateDictionaryWidgetIsVisible()
        {
            // Find the widget container to verify it exists
            _widgetElement = _defaultWait.Until(d =>
            {
                var results = d.FindElements(By.CssSelector("div.lr_container"));
                return results.FirstOrDefault();
            });

            Assert.IsNotNull(_widgetElement);
        }

        [Then(@"the definition for (.*) should be displayed")]
        public void ValidateDefinition(string word)
        {
            IWebElement input =
                _defaultWait.Until(d => _widgetElement.FindElement(By.CssSelector($"input[value=\"{word}\"]")));

            Assert.AreEqual("text", input.GetAttribute("type"));
            Assert.AreEqual("Enter a word", input.GetAttribute("placeholder"));
        }
    }
}
