Feature: Create a store
	I can create a new store so that I can
	perform authorisation

Scenario: Create XML Store
	Given I have a file path
	And there is on file at the path
	When I create a new store at the path
	Then I can open the new store
