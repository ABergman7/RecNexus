using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace PickUpSportsSpecFlow
{
    [Binding]
    public class VenueOwnerAvailability : IDisposable
    {

        private readonly IWebDriver _driver;

        public VenueOwnerAvailability() 
        {
            _driver = new ChromeDriver();
        }

        [Given(@"I have logged in")]
        public void GivenIHaveLoggedIn()
        {
            _driver.Navigate().GoToUrl("https://pickupsports-production.azurewebsites.net/Account/Login");

            IWebElement username = _driver.FindElement(By.Id("Email"));
            username.Clear();
            username.SendKeys("");

            IWebElement password = _driver.FindElement(By.Id("Password"));
            password.Clear();
            password.SendKeys("");


            _driver.FindElement(By.Id("login-button")).Click();
        }
        
        [Given(@"I have navigate to the create game page")]
        public void GivenIHaveNavigateToTheCreateGamePage()
        {
            _driver.Navigate().GoToUrl("https://pickupsports-production.azurewebsites.net/Game/CreateGame");
        }

        [When(@"I select Bryan Johnston Park as the venue")]
        public void WhenISelectBryanJohnstonParkAsTheVenue()
        {
            //selecting the dropdown
            var venues = _driver.FindElement(By.Name("VenueId"));
            var selectVenues = new SelectElement(venues);

            //select certain venue on dropdown
            selectVenues.SelectByText("Bryan Johnston Park");
        }
        
        [Then(@"I should see a message telling me that there are no owners for this venue")]
        public void ThenIShouldSeeAMessageTellingMeThatThereAreNoOwnersForThisVenue()
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
