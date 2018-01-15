@bing
Feature: BingSearch
	In order to validate the Bing search service
	as a first time user of Selenium
	I want to perform a search

Scenario: Perform a Bing Search
	Given I navigate to www.bing.com
	When I search for kittens
	Then the search engine should return valid search results

Scenario: Dictionary Search
	Given I navigate to www.bing.com
	When I search for define: relativity
	Then the search engine should show the dictionary widget
	And the definition for relativity should be displayed

Scenario Outline: Unit Conversions
	Given I navigate to www.bing.com
	When I convert <src-amount> <src-unit> to <dest-unit>
	Then the search engine should show the conversion widget for <type>
	And the conversion result should be <dest-amount> <dest-unit>

Examples:
	| type            | src-amount | src-unit  | dest-amount | dest-unit |
	| length          | 5          | kilometer | 3.10685596  | mile      |
	| mass            | 5          | kilogram  | 11.0231131  | pound     |
	| digital storage | 1000       | kilobyte  | 1           | megabyte  |