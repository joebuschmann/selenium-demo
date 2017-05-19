Feature: GoogleSearch
	In order to validate Google's search service
	As a first time user of Selenium
	I want to perform a search

Scenario: Perform a Google Search
	Given I navigate to www.google.com
	When I search for kittens
	Then Google should return valid search results