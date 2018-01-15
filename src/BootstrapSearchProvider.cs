using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoDi;
using TechTalk.SpecFlow;

namespace SeleniumAndSpecflow
{
    [Binding]
    public class BootstrapSearchProvider
    {
        private readonly IObjectContainer _objectContainer;
        private readonly List<string> _tags = new List<string>();

        public BootstrapSearchProvider(IObjectContainer objectContainer, ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _objectContainer = objectContainer;
            _tags.AddRange(scenarioContext.ScenarioInfo.Tags);
            _tags.AddRange(featureContext.FeatureInfo.Tags);
        }

        [BeforeScenario]
        public void LoadSearchProvider()
        {
            if (_tags.Contains("bing"))
                _objectContainer.RegisterTypeAs<BingSearchProvider, ISearchProvider>();
            else if (_tags.Contains("google"))
                _objectContainer.RegisterTypeAs<GoogleSearchProvider, ISearchProvider>();
        }
    }
}
