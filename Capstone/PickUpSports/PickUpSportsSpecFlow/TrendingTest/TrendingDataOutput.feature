Feature: TrendingDataOutput
	In order to test trending data from trending
	As a developer
	I want to ensure that the users have a suggested venue per sport 

@nosport
Scenario: Trending should return to default screen
	Given I have navigated to the trending page
	When I select Select a Sport as the sport
	Then I should see a message telling me that no sport has been selected
