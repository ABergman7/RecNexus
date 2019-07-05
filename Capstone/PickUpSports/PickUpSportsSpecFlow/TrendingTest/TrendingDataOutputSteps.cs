using System;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace PickUpSportsSpecFlow
{
    [Binding]
    public class TrendingDataOutputSteps : IDisposable
    {
        private readonly IWebDriver _driver;

        public TrendingDataOutputSteps()
        {
            _driver = new ChromeDriver();
        }

        [Given(@"I have navigated to the trending page")]
        public void GivenIHaveNavigatedToTheTrendingPage()
        {
             _driver.Navigate().GoToUrl("https://pickupsports-production.azurewebsites.net/Trending/index");
        }
        
        [When(@"I select Select a Sport as the sport")]
        public void WhenISelectSelectASportAsTheSport()
        {
            //selecting the dropdown
            var sports = _driver.FindElement(By.Name("SportName"));
            var selectSports = new SelectElement(sports);

            //select certain venue on dropdown
            selectSports.SelectByText("Select a Sport");
        }
        
        [Then(@"I should see a message telling me that no sport has been selected")]
        public void ThenIShouldSeeAMessageTellingMeThatNoSportHasBeenSelected()
        {
            //Wait for the partial view to show
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

            //expect an error message that owner is not there
            var element = _driver.FindElement(By.Id("error-msg"));
            element.Displayed.Should().BeTrue();
        }

        public void Dispose()
        {
            if (_driver != null)
            {
                _driver.Dispose();
            }
        }
    }
}
