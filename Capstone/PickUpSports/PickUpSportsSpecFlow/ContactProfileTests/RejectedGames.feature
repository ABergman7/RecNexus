Feature: RejectedGames
	In order to see which games users created have been rejected by the venue owner
	As a visitor
	I want to see a column of rejected games that were created by the user

@rejectedgame
Scenario: Rejected Game Column
	Given I have navigated to the site as a visitor
	When I visit his page
	Then I should see a column that shows the games that he created that got rejected 
