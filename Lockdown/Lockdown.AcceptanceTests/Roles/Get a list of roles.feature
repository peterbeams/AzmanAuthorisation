Feature: Get a list of roles
	As a policy administrator
	I can get a list of roles
	so that I can see the roles I can apply

Scenario: Two Roles
	Given I have an azman store
	And the store has an application called MyApp
	And the store has an role called role1
	And the store has an role called role2
	When I open the store
	And I get the list of roles
	Then I get a list of roles with 2 items in it
	And I get a list with a role called role1 in it
	And I get a list with a role called role2 in it

Scenario: No Roles
	Given I have an azman store
	And the store has an application called MyApp
	And the store has no tasks
	When I open the store
	And I get the list of roles
	Then I get an empty list of tasks
