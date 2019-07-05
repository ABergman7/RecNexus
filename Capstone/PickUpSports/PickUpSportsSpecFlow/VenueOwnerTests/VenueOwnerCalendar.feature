Feature: VenueOwnerCalendar
	In order to schedule and view any games at my venue
	As a venue owner
	I want to see a calendar view of all the game at my venue

@viewcalendar
Scenario: Owner Calendar View
	Given I have navigated to the site
	And I have logged in as an owner of Real Sports Venue
	When I press View Your Profile on the navbar
	Then I should be shown my account detail with the calendar where I can accept and reject games
