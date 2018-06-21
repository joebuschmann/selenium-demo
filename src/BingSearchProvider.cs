using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumAndSpecflow
{
    public class BingSearchProvider : ISearchProvider
    {
        private readonly IWebDriver _webDriver;
        private readonly IWait<IWebDriver> _defaultWait;
        private string _searchTerm;
        private IWebElement _widgetElement;

        public BingSearchProvider(IWebDriver webDriver, IWait<IWebDriver> defaultWait)
        {
            _webDriver = webDriver;
            _defaultWait = defaultWait;
        }

		public void InitializeBrowser()
		{
			_webDriver.Navigate().GoToUrl("https://www.bing.com");
		}

		public void Search(string searchTerm)
        {
            _searchTerm = searchTerm;

            IWebElement searchInput = _webDriver.FindElement(By.CssSelector("input#sb_form_q"));
            searchInput.SendKeys(searchTerm);

            IWebElement searchForm = _webDriver.FindElement(By.CssSelector("form#sb_form"));
            searchForm.Submit();
        }

        public void ValidateConversionResult(string destAmount, string destUnit)
        {
            // Make sure the correct amount is displayed
            IWebElement amountElement =
                _defaultWait.Until(d => _widgetElement.FindElement(By.CssSelector("input#uc_rv")));

            Assert.AreEqual(destAmount, amountElement.GetAttribute("value"));

            // Make sure the correct unit is selected
            IWebElement selectedOption = _defaultWait.Until(d =>
                _widgetElement.FindElement(By.CssSelector("select#uc_rt option:checked")));

            Assert.IsNotNull(selectedOption);
            Assert.AreEqual(destUnit.ToLower(), selectedOption.Text.ToLower());
        }

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

        public void ValidateDefinition(string definition)
        {
			// Validate the search term is displayed
            IWebElement searchInput = _defaultWait.Until(d => d.FindElement(By.CssSelector("input#sb_form_q")));
            Assert.AreEqual(_searchTerm, searchInput.GetAttribute("value"));

			// Make sure the definition is displayed
	        // The definition span isn't easy to get. Query for all spans inside a list and check them for the definition.
	        IWebElement elementWithDefinition =
		        _defaultWait.Until(d =>
		        {
			        IEnumerable<IWebElement> elements =
				        _widgetElement.FindElements(By.CssSelector("div.WordContainer ol li div"));

			        return elements.FirstOrDefault(e => e.Text == definition);
		        });

	        Assert.IsNotNull(elementWithDefinition);
		}

        public void ValidateDictionaryWidgetIsVisible()
        {
            _widgetElement = _defaultWait.Until(d => d.FindElement(By.CssSelector("div.WordContainer")));
        }

        public void ValidateSearchResults()
        {
            IWebElement searchInput = _defaultWait.Until(d => d.FindElement(By.CssSelector("input#sb_form_q")));
            Assert.AreEqual(_searchTerm, searchInput.GetAttribute("value"));

            IEnumerable<IWebElement> resultsList =
                _defaultWait.Until(d => d.FindElement(By.Id("b_results")).FindElements(By.TagName("li")));
            Assert.IsTrue(resultsList.Any());
        }
    }
}