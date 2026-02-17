Feature: UsersTests

The system should allow the administrator to manage user accounts, including creating, listing and deleting users. A common user should be able to view the list of users but not create or delete them.

Background:
	Given I navigate to the main page
	And I verify that the login form is displayed

@Ignore
@Users @E2E
# Scenario with a multiple When–Then structure is not considered as a best practice. 
# I'll leave it because it shows the usage of ignore tag.
Scenario: A user can register a new account successfully and the administrator can see the new user in the users list and delete it
	When I navigate to the registration page
	And I verify that the registration form is displayed
	And I register a new user with valid details
	Then I should see the dashboard of the user
	And I should be able to logout successfully

	When I login with admin credentials
	And I verify the dashboard shows admin details
	And I navigate to the users page
	Then the new user should be present in the users list

	When I delete the created user
	Then the user should no longer be present in the users list
	And I should be able to logout successfully

	When I try to login with the deleted user's credentials
	Then I should still be on the login page
	And I should see an error message with the following text "Invalid email or password"


@Users @E2E
# Scenario with a single When–Then structure final version
Scenario: Verify a registered user can be deleted by an admin user and the user cannot login afterwards
	Given I register a new user
	And I login with admin credentials
	And I navigate to the users page

	When I delete the created user
	And I log out successefuly

	Then I login with the deleted user's credentials
	And I should still be on the login page
	And I should see an error message with the following text "Invalid email or password"


@Users
Scenario: Verify admin user can add a new user and the new user can login successfully
	Given I login with admin credentials
	And I navigate to the users page
	When I add a new user with valid details
	And I log out successefuly
	Then I login with the new user's credentials
	And I should see the dashboard of the added user
	And I should be able to logout successfully


@Users @UsersApi @FromSession
Scenario: Verify newly created users are displayed in the users list
	Given I create a user successfully
	When I login with valid credentials
	And I navigate to the users page
	Then I should see the created user in the users list