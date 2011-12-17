Feature: Get a list of tasks
	As a policy administrator
	I can get a list of tasks
	so that I can see tasks that are in my policies

Scenario: Two Tasks
	Given I have an azman store
	And the store has an application called MyApp
	And the store has an task called task1
	And the store has an task called task2
	When I open the store
	And I get the list of tasks
	Then I get a list of tasks with 2 items in it
	And I get a list with a task called task1 in it
	And I get a list with a task called task2 in it

Scenario: No Roles
	Given I have an azman store
	And the store has an application called MyApp
	And the store has no tasks
	When I open the store
	And I get the list of tasks
	Then I get an empty list of tasks
