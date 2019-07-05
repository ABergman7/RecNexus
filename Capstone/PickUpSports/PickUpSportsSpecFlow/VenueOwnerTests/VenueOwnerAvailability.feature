Feature: VenueOwnerAvailability
	In order to test owner availability for the venue
	As a developer
	I want to ensure that the users know which venues have owners and which don't

@noowner
Scenario: Venues without owners should display a message accordingly 
	Given I have logged in 
	And I have navigate to the create game page
	When I select Bryan Johnston Park as the venue
	Then I should see a message telling me that there are no owners for this venue

