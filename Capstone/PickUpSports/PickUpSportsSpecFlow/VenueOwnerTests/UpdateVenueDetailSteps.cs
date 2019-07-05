using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace PickUpSportsSpecFlow
{
    [Binding]
    public class UpdateVenueDetailSteps : IDisposable
    {
        private readonly IWebDriver _driver;

        public UpdateVenueDetailSteps()
        {
            _driver = new ChromeDriver();
        }

        [Given(@"I have logged in as a venue owner for Real Sports Venue")]
        public void GivenIHaveLoggedInAsAVenueOwnerForRealSportsVenue()
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
        
        [Given(@"I have navigated to my venue's detail page")]
        public void GivenIHaveNavigatedToMyVenueSDetailPage()
        {
            _driver.Navigate().GoToUrl("https://pickupsports-production.azurewebsites.net/Venue/Details/67");
        }
        
        [When(@"I press edit venue")]
        public void WhenIPressEditVenue()
        {
           _driver.FindElement(By.LinkText("Edit Venue Details")).Click();
        }
        
        [Then(@"I should be taken to a page where I can edit all my venue fields")]
        public void ThenIShouldBeTakenToAPageWhereICanEditAllMyVenueFields()
        {
            var titleElement = _driver.FindElement(By.XPath("//h2[@id='Name']"));
            var addressElement = _driver.FindElement(By.XPath("//div[@id='AddressInformation']/b"));
            var hoursElement = _driver.FindElement(By.XPath("//div[@id='BusinessHours']/b"));

            titleElement.Displayed.Should().BeTrue();
            addressElement.Displayed.Should().BeTrue();
            hoursElement.Displayed.Should().BeTrue();
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
