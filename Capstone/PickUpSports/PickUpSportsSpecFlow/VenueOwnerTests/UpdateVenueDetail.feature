Feature: UpdateVenueDetail
	In order to test the ability to edit venue details
	As a developer
	I want to make sure the owner can easily update their venue details

@editvenue
Scenario: Venue Detail Edit
	Given I have logged in as a venue owner for Real Sports Venue
	And I have navigated to my venue's detail page
	When I press edit venue
	Then I should be taken to a page where I can edit all my venue fields 
