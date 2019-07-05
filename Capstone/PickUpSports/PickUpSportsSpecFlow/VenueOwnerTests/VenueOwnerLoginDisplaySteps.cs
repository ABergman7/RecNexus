using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace PickUpSportsSpecFlow.VenueOwnerTests
{
    [Binding]
    public class VenueOwnerLoginDisplaySteps : IDisposable
    {
        private readonly IWebDriver _driver;

        public VenueOwnerLoginDisplaySteps() 
        {
            _driver = new ChromeDriver();
        }

        [Given(@"I have navigated to the website's welcome screen")]
        public void GivenIHaveNavigatedToTheWebsiteSWelcomeScreen()
        {
            _driver.Navigate().GoToUrl("https://pickupsports-production.azurewebsites.net/");
        }
        
        [Given(@"I have clicked on the login navbar item")]
        public void GivenIHaveClickedOnTheLoginNavbarItem()
        {
            _driver.FindElement(By.Id("loginLink")).Click();
        }
        
        [When(@"I click the Venue Owner link")]
        public void WhenIClickTheVenueOwnerLink()
        {
            _driver.FindElement(By.LinkText("Log in as Venue Owner")).Click();
        }
        
        [Then(@"It should take me to a venue owner login portal")]
        public void ThenItShouldTakeMeToAVenueOwnerLoginPortal()
        {
            var title = _driver.FindElement(By.Id("venueOwner-login"));
            var email = _driver.FindElement(By.Id("Email"));
            var password = _driver.FindElement(By.Id("Password"));

            title.Text.Should().Be("VENUE OWNER LOG IN");

            title.Displayed.Should().BeTrue();
            email.Displayed.Should().BeTrue();
            password.Displayed.Should().BeTrue();
        }
        
        [Then(@"I should be able to successfully login")]
        public void ThenIShouldBeAbleToSuccessfullyLogin()
        {
            IWebElement username = _driver.FindElement(By.Id("Email"));
            username.Clear();
            username.SendKeys("");

            IWebElement password = _driver.FindElement(By.Id("Password"));
            password.Clear();
            password.SendKeys("");


            _driver.FindElement(By.Id("login-button")).Click();
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
