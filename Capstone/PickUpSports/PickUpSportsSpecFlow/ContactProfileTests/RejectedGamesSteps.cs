using System;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;


namespace PickUpSportsSpecFlow.ContactProfileTests
{
    [Binding]
    public class RejectedGamesSteps
    {
        private readonly IWebDriver _driver;

        public RejectedGamesSteps()
        {
            _driver = new ChromeDriver();
        }

        [Given(@"I have navigated to the site as a visitor")]
        public void GivenIHaveNavigatedToTheSiteAsAVisitor()
        {
            _driver.Navigate().GoToUrl("https://pickupsports-production.azurewebsites.net/");
        }
        
        [When(@"I visit his page")]
        public void WhenIVisitHisPage()
        {
            _driver.Navigate().GoToUrl("https://pickupsports-production.azurewebsites.net/Contact/PlayerProfile/34");
        }
        
        [Then(@"I should see a column that shows the games that he created that got rejected")]
        public void ThenIShouldSeeAColumnThatShowsTheGamesThatHeCreatedThatGotRejected()
        {
            var rejectedGameColumn = _driver.FindElement(By.XPath("//div[3]/center"));
            rejectedGameColumn.Displayed.Should().BeTrue();
        }
    }
}
