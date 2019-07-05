using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace PickUpSportsSpecFlow
{
    [Binding]
    public class VenueOwnerCalendarSteps : IDisposable
    {
        private readonly IWebDriver _driver;

        public VenueOwnerCalendarSteps()
        {
            _driver = new ChromeDriver();
        }

        [Given(@"I have navigated to the site")]
        public void GivenIHaveNavigatedToTheSite()
        {
            _driver.Navigate().GoToUrl("https://pickupsports-production.azurewebsites.net/");
        }
        
        [Given(@"I have logged in as an owner of Real Sports Venue")]
        public void GivenIHaveLoggedInAsAnOwnerOfRealSportsVenue()
        {
            _driver.Navigate().GoToUrl("https://pickupsports-production.azurewebsites.net/Account/VenueOwnerLogin");

            IWebElement username = _driver.FindElement(By.Id("Email"));
            username.Clear();
            username.SendKeys("");

            IWebElement password = _driver.FindElement(By.Id("Password"));
            password.Clear();
            password.SendKeys("");


            _driver.FindElement(By.Id("login-button")).Click();
        }
        
        [When(@"I press View Your Profile on the navbar")]
        public void WhenIPressViewYourProfileOnTheNavbar()
        {
            _driver.FindElement(By.XPath("//a[contains(text(),'View your profile')]")).Click();
        }

        [Then(@"I should be shown my account detail with the calendar where I can accept and reject games")]
        public void ThenIShouldBeShownMyAccountDetailWithTheCalendarWhereICanAcceptAndRejectGames()
        {
            //Wait for the partial view to show
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

            var calendar = _driver.FindElement(By.XPath("//div[@id='calender']/div"));

            calendar.Displayed.Should().BeTrue();
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
