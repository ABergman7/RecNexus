using System;
using OpenQA.Selenium;
using FluentAssertions;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace PickUpSportsSpecFlow.VenueDetailTests
{
    [Binding]
    public class VenueDetailsDisplaySteps : IDisposable
    {
        private readonly IWebDriver _driver;

        public VenueDetailsDisplaySteps()
        {
            _driver = new ChromeDriver();
        }

        [Given(@"I am a guest user to the website")]
        public void GivenIAmAGuestUserToTheWebsite()
        {
            _driver.Navigate().GoToUrl("https://pickupsports-production.azurewebsites.net/");
        }
        
        [Given(@"I have selected the Venue link on the homepage")]
        public void GivenIHaveSelectedTheVenueLinkOnTheHomepage()
        {
            _driver.FindElement(By.LinkText("View Venues")).Click();
        }
        
        [When(@"I click on the details for Bryan Johnston Park")]
        public void WhenIClickOnTheDetailsForBryanJohnstonPark()
        {
            _driver.FindElement(By.XPath("//a[contains(@href, '/Venue/Details/1')]")).Click();
        }
        
        [Then(@"I should see all the necessary details about the venue")]
        public void ThenIShouldSeeAllTheNecessaryDetailsAboutTheVenue()
        {
            var name =_driver.FindElement(By.Id("Name")); 
            var addressInformation = _driver.FindElement(By.Id("AddressInformation")); 
            var averageRating = _driver.FindElement(By.Id("AverageRating")); 
            var businessHours = _driver.FindElement(By.Id("BusinessHours")); 
            var reviewLink = _driver.FindElement(By.LinkText("Leave a review!")); 
            var reviews = _driver.FindElement(By.Id("Reviews")); 
            var games = _driver.FindElement(By.Id("Games"));
            var venueOwnerLink = _driver.FindElement(By.LinkText("click here"));

            //Display Assertions 
            name.Displayed.Should().BeTrue();
            addressInformation.Displayed.Should().BeTrue();
            averageRating.Displayed.Should().BeTrue();
            businessHours.Displayed.Should().BeTrue();
            reviewLink.Displayed.Should().BeTrue();
            reviews.Displayed.Should().BeTrue();
            games.Displayed.Should().BeTrue();
            venueOwnerLink.Displayed.Should().BeTrue();

            //Display Text Assertions
            name.Text.Should().Be("BRYAN JOHNSTON PARK");
            addressInformation.Text.Should().Contain("400 Mildred Ln SE");
            averageRating.Text.Should().Contain("Average Review Rating");
            businessHours.Text.Should().Contain("Hours of Operation");


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
