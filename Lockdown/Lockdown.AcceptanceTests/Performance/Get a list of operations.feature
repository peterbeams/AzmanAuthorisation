Feature: Get List Of Operations	

@performance
Scenario: Outright speed
	Given I have an azman store
	And the store has an application called MyApp
	And the store has an operation X with id 1
	And the store has an operation Y with id 2	
	When I open the store 
	And I get the list of operations 100000 times
	Then the test takes less than 7000ms

@performance
Scenario: Parallel outright speed
	Given I have an azman store
	And the store has an application called MyApp
	And the store has an operation X with id 1
	And the store has an operation Y with id 2	
	When I open the store 
	And I get the list of operations 100000 times in parallel
	Then the test takes less than 2000ms
