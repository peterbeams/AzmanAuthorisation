Feature: Get List Of Operations	

Scenario: Outright speed
	Given I have an azman store
	And the store has an application called MyApp
	And the store has an operation X with id 1
	And the store has an operation Y with id 2	
	When I open the store 
	And I get the list of operations 1000 times
	Then the test takes less than 500ms

Scenario: Parallel outright speed
	Given I have an azman store
	And the store has an application called MyApp
	And the store has an operation X with id 1
	And the store has an operation Y with id 2	
	When I open the store 
	And I get the list of operations 1000 times in parallel
	Then the test takes less than 500ms
