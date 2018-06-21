@google
Feature: GoogleSearch
	In order to find information on the internet
	as a user of the Google search engine
	I want to perform a search

Scenario: Perform a Basic Search
	Given a browser loaded with the search provider's web page
	When I search for kittens
	Then the search engine should return valid search results

Scenario: Dictionary Search
	Given a browser loaded with the search provider's web page
	When I search for define: relativity
	Then the search engine should show the dictionary widget
	And the following definition should be returned
		"""
		the absence of standards of absolute and universal application.
		"""

Scenario Outline: Unit Conversions
	Given a browser loaded with the search provider's web page
	When I convert <src-amount> <src-unit> to <dest-unit>
	Then the search engine should show the conversion widget for <type>
	And the conversion result should be <dest-amount> <dest-unit>

Examples:
	| type            | src-amount | src-unit  | dest-amount | dest-unit |
	| length          | 5          | kilometer | 3.10686     | mile      |
	| mass            | 5          | kilogram  | 11.0231     | pound     |
	| digital storage | 1000       | kilobyte  | 1           | megabyte  |
