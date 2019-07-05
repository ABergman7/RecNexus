Feature: VenueOwnerLoginDisplay
	In order to login as a venue owner 
	As a user who is a venue owner
	I want to have a different login portal than the other users 

@ownerlogin
Scenario: Venue Owner Login
	Given I have navigated to the website's welcome screen
	And I have clicked on the login navbar item
	When I click the Venue Owner link
	Then It should take me to a venue owner login portal
	And  I should be able to successfully login
