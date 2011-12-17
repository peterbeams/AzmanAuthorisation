Feature: Get a list of operations
	In order to see what operations can be authorised
	I can get the list of operations
	so that I can check if the operations are authorised

Scenario: Two operations
	Given I have an azman store
	And the store has an application called MyApp
	And the store has an operation X with id 1
	And the store has an operation Y with id 2	
	When I open the store 
	And I get the list of operations
	Then I get a list with an operation called X with id 1
	And I get a list with an operation called Y with id 2
