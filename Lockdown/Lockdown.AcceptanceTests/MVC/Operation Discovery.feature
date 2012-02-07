Feature: Operation Discovery
	In order to authorise calls to MVC controller actions
	the authorisation service will discover controller actions
	and create operations for them

Scenario: Single Controller
	Given I have a MVC application called Achme.MyApp
	And the application has a controller called Home Controller
	And the Home Controller has an action called Index
	And the Home Controller has an action called About
	When I discover operations in the application
	Then an operation called Achme.MyApp.Controllers.HomeController.Index should be found
	Then an operation called Achme.MyApp.Controllers.HomeController.About should be found


Scenario: Single Controller with Post Action
	Given I have a MVC application
	And the application has a controller called Home Controller
	And the Home Controller has an action called Index
	And the Home Controller has an POST action called Index
	When I discover operations in the application
	Then an operation called Achme.MyApp.Controllers.HomeController.Index should be found
	Then an operation called Achme.MyApp.Controllers.HomeController.Index[POST] should be found

Scenario: Two Controllers
	Given I have a MVC application called Achme.MyApp
	And the application has a controller called Home Controller
	And the Home Controller has an action called Index
	And the application has a controller called Product Controller
	And the Product Controller has an action called Index
	When I discover operations in the application
	Then an operation called Achme.MyApp.Controllers.HomeController.Index should be found
	Then an operation called Achme.MyApp.Controllers.ProductController.About should be found