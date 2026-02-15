Feature: RegisterTests

As a user I would like to be able to register when visiting the page for the first time and to be able to 
login with the created credentials.

@Register @DeleteRegisteredUser
Scenario: Verify user is able to register successfully
	Given I navigate to the main page
	And I register new user with valid details
	Then I should see the created user is logged successfully
	And I should be able to logout successfully
