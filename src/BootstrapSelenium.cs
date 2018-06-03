using System;
using System.Configuration;
using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace SeleniumAndSpecflow
{
    [Binding]
    public class BootstrapSelenium : IDisposable
    {
        private readonly IObjectContainer _objectContainer;
        private IWait<IWebDriver> _defaultWait = null;
        private IWebDriver _webDriver = null;

        public BootstrapSelenium(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void LoadDriverAndDefaultWait()
        {
            // Create the driver and add to the container
            string driverType = ConfigurationManager.AppSettings["driver"].ToLower();

            switch (driverType)
            {
                case "chrome":
                    _webDriver = new ChromeDriver();
                    break;
                case "firefox":
                    _webDriver = new FirefoxDriver();
                    break;
                case "ie":
                    _webDriver = new InternetExplorerDriver();
                    break;
                case "edge":
                    _webDriver = new EdgeDriver();
                    break;
                default:
                    _webDriver = new ChromeDriver();
                    break;
            }

            _objectContainer.RegisterInstanceAs(_webDriver, typeof (IWebDriver));

            // Create the default wait and add to the container
            int timeout;
            int pollInterval;

            if (!int.TryParse(ConfigurationManager.AppSettings["timeout"], out timeout))
                timeout = 5000;

            if (!int.TryParse(ConfigurationManager.AppSettings["pollinterval"], out pollInterval))
                pollInterval = 100;

            _defaultWait = new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(timeout))
            {
                PollingInterval = TimeSpan.FromMilliseconds(pollInterval)
            };

            _objectContainer.RegisterInstanceAs(_defaultWait, typeof(IWait<IWebDriver>));
        }

        public void Dispose()
        {
            if (_webDriver != null)
            {
                _webDriver.Quit();
                _webDriver = null;
                _defaultWait = null;
            }
        }
    }
}
