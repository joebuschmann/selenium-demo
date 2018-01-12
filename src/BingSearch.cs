using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace SeleniumAndSpecflow
{
    [Binding, Scope(Feature = "BingSearch")]
    public class BingSearch
    {
        private IWebDriver _webDriver;
        private IWait<IWebDriver> _defaultWait;
        private string _searchTerm;
        private IWebElement _widgetElement;

        public BingSearch(IWebDriver webDriver, IWait<IWebDriver> defaultWait)
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

            IWebElement searchInput = _webDriver.FindElement(By.CssSelector("input#sb_form_q"));
            searchInput.SendKeys(searchTerm);

            IWebElement searchForm = _webDriver.FindElement(By.CssSelector("form#sb_form"));
            searchForm.Submit();
        }

        [Then(@"Bing should return valid search results")]
        public void ValidateSearchResults()
        {
            IWebElement searchInput = _defaultWait.Until(d => d.FindElement(By.CssSelector("input#sb_form_q")));
            Assert.AreEqual(_searchTerm, searchInput.GetAttribute("value"));

            IEnumerable<IWebElement> resultsList =
                _defaultWait.Until(d => d.FindElement(By.Id("b_results")).FindElements(By.TagName("li")));
            Assert.IsTrue(resultsList.Any());
        }

        [When(@"I convert (.*) (.*) to (.*)")]
        public void Convert(string srcAmount, string srcUnit, string destUnit)
        {
            string searchTerm = $"convert {srcAmount} {srcUnit} to {destUnit}";
            Search(searchTerm);
        }

        [Then(@"Bing should show the conversion widget for (.*)")]
        public void ValidateConversionWidgetIsVisible(string type)
        {
            // Find the widget container to verify it exists
            _widgetElement = _defaultWait.Until(d => d.FindElement(By.Id("rich_uc")));
            IWebElement titleElement =
                _defaultWait.Until(d => _widgetElement.FindElement(By.CssSelector("h2.b_topTitle")));

            Assert.AreEqual("convert units", titleElement.Text.ToLower());

            // Make sure the correct measurement type is selected in the form
            IWebElement selectedOption = _defaultWait.Until(d =>
                _widgetElement.FindElement(By.CssSelector("select#uc_st option:checked")));

            Assert.IsNotNull(selectedOption);
            Assert.AreEqual(type.ToLower(), selectedOption.Text.ToLower());
        }

        [Then(@"the conversion result should be (.*) (.*)")]
        public void ValidateConversionResult(string destAmount, string destUnit)
        {
            // Make sure the correct amount is displayed
            IWebElement amountElement = _defaultWait.Until(d =>
            {
                return _widgetElement.FindElement(By.CssSelector("input#uc_rv"));
            });

            Assert.AreEqual(destAmount, amountElement.GetAttribute("value"));

            // Make sure the correct unit is selected
            IWebElement selectedOption = _defaultWait.Until(d =>
            {
                return _widgetElement.FindElement(By.CssSelector("select#uc_rt option:checked"));
            });

            Assert.IsNotNull(selectedOption);
            Assert.AreEqual(destUnit.ToLower(), selectedOption.Text.ToLower());
        }

        [Then(@"Bing should show the dictionary widget")]
        public void ValidateDictionaryWidgetIsVisible()
        {
            _widgetElement = _defaultWait.Until(d => d.FindElement(By.CssSelector("div.WordContainer")));
        }

        [Then(@"the definition for (.*) should be displayed")]
        public void ValidateDefinition(string word)
        {
            IWebElement searchInput = _defaultWait.Until(d => d.FindElement(By.CssSelector("input#sb_form_q")));
            Assert.AreEqual(word, searchInput.GetAttribute("value").Replace("define:", "").Trim());
        }

    }
}
