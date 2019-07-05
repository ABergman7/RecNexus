Feature: VenueDetailsDisplay
	In order to see all the details of the venue placed back in order after updating
	As a developer
	I want to ensure the page is reloaded correctly and all the necessary elements are added back

@mytag
Scenario: Check Venue Details 
	Given I am a guest user to the website
	And I have selected the Venue link on the homepage
	When I click on the details for Bryan Johnston Park
	Then I should see all the necessary details about the venue 
