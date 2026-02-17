@UsersApi
Feature: UsersApiTests

CRUD operations for users endpoints

@FromSession
Scenario: Get users by id returns the correct user
	Given I make a get request to users endpoint with id 1
	Then the response status code should be 200
	And users response should contain the following data:
		| Id | FirstName | SirName    | Title | Email                | Password | Country  | City  | IsAdmin |
		|  1 | Admin     | Automation | Mr.   | admin@automation.com | pass123  | Bulgaria | Sofia |       1 |


@Negative @FromSession
Scenario: Get users by id returns the correct error for invalid user ID
	Given I make a get request to users endpoint with id 0
	Then the response status code should be 404
	And the response should contain the following error message "User not found"


@Negative
# Actual: API returns an HTML 404 page instead of a JSON error response
Scenario: Get users by id with negative ID value returns error
	Given I make a get request to users endpoint with id -1
	Then the response status code should be 404
	And the response should contain the following error message "Not Found"


Scenario: Get all users returns a non-empty list of valid users
	Given I make a get request to users endpoint
	Then the response status code should be 200
	And users response should contain non-empty list of users
	And each user in the list should have valid data


@FromSession
Scenario: Create user with valid data returns the created user
	Given I make a post request to users endpoint with the following data:
		| Field     | Value               |
		| Title     | Mr.                 |
		| FirstName | Ivan                |
		| SirName   | Ivanov              |
		| Country   | Bulgaria            |
		| City      | Sofia               |
		| Email     | ivan@automation.com |
		| Password  | pass123             |
		| IsAdmin   |                   0 |
	Then the response status code should be 200
	And create users response should contain the following data:
		| Field     | Value    |
		| Title     | Mr.      |
		| FirstName | Ivan     |
		| SirName   | Ivanov   |
		| Country   | Bulgaria |
		| City      | Sofia    |
		| IsAdmin   |        0 |


@Negative
Scenario Outline: Create user with invalid data returns validation error
	Given I make a post request to users endpoint with invalid data "<field>" "<value>"
	Then the response status code should be 400
	And the response should contain the following error message "<ErrorMessage>"

Examples:
	| field     | value        | ErrorMessage                                 |
	| Title     |              | Title is required                            |
	| FirstName |              | First Name is required                       |
	| SirName   |              | Sir Name is required                         |
	| Country   |              | Country is required                          |
	| Email     |              | Email is required                            |
	| FirstName | Ana-Maria    | First name cannot contain special characters |
	| SirName   | O'Conner     | Sir name cannot contain special characters   |
	| Email     | test.com     | Invalid email format                         |
	| Email     | @test.com    | Invalid email format                         |
	| Email     | test@test    | Invalid email format                         |
	| Email     | test@@abv.bg | Invalid email format                         |
	| Email     | .@test.com   | Invalid email format                         | @Issue # Actual: 200 OK on first run even email is not valid, and afterthat 409 CONFLICT (duplicate email)
	
	
@Negative @FromSession
Scenario: Create user with empty data returns correct errors for missing mandatory fields
	Given I make a post request to users endpoint with empty mandatory fields
	Then the response status code should be 400
	And response should contain error messages:
	| ErrorMessage                                 |
	| Title is required                            |
	| First Name is required                       |
	| Sir Name is required                         |
	| Country is required                          |
	| Email is required                            |


@Negative
Scenario: Create user with existing email returns conflict
	Given I create a new user via the API
	When I try to create another user with the same email
	Then the response status code should be 409
	And the response should contain the following error message "User with such email already exists"


@Negative @Issue
# API returns 500 Internal Server Error, but invalid title should produce a client-side validation error (400 Bad Request) with an appropriate message
Scenario Outline: Create user with invalid title returns validation error
	Given I make a post request to users endpoint with Title "<value>"
	Then the response status code should be 400
	And the response should contain the following error message "Title is not valid"

Examples:
	| value |
	| Mr    |
	| Mrs   |
	| Miss  |


@Negative
Scenario Outline: Create user with city not belonging to the specified country returns validation error
	Given I make a post request to users endpoint with Country "<country>" and City "<city>"
	Then the response status code should be 400
	And the response should contain the following error message "City does not belong to the specified country"

Examples:
	| country  | city              |
	| Bulgaria | London            |
	| Bulgaria | Ruse              | # The city does not exist in database
	| Bulgaria | Gorna Oryahovitsa | # The city does not exist in database
	| Germany  | Paris             |

	
# City is optional when creating a user via the Api - it is not a required field
Scenario: Create user without city succeeds
	Given I make a post request to users endpoint with Country "Italy" and City ""
	Then the response status code should be 200


@Negative @Issue
Scenario: Create user with unknown country for the Api returns validation error with not proper message
	Given I make a post request to users endpoint with Country "Netherlands" and City "Amsterdam"
	Then the response status code should be 400
	And the response should contain the following error message "City does not belong to the specified country"


Scenario: Delete user by id removes the user successfully and the user cannot be found afterwards
	Given I create a new user via the API
	When I delete that user
	Then the response status code should be 200
	And the response should contain the following message "User deleted successfully"
	And I make a get request to users endpoint with that id
	And the response status code should be 404
	And the response should contain the following error message "User not found"


@Negative
Scenario Outline: Delete users by id returns the correct error for non-existing user ID
	When I make a Delete request to users endpoint with id <id>
	Then the response status code should be 404
	And the response should contain the following error message "User not found"

Examples:
	| id        |
	|         0 |
	| 123456789 |


@Negative
# Actual: API returns an HTML 404 page instead of a JSON error response
Scenario: Delete users by id with negative ID value returns error
	When I make a Delete request to users endpoint with id -1
	Then the response status code should be 404
	And the response should contain the following error message "Not Found"


Scenario: Update user by id with valid data returns the updated user
	Given I create a new user via the API
	When I update that user with valid data
	Then the response status code should be 200
	And the updated user should have the new data


@Negative
Scenario Outline: Update user with invalid data returns validation error
	Given I create a new user via the API
	When I update that user with invalid data "<field>" "<value>"
	Then the response status code should be 400
	And the response should contain the following error message "<ErrorMessage>"

Examples:
	| field     | value          | ErrorMessage                                  | # Actual
	| Email     |                | Email is required                             |
	| Title     |                | Title is required                             |
	| FirstName |                | First Name is required                        |
	| SirName   |                | Sir Name is required                          |
	| Country   |                | Country is required                           |
	| FirstName | Ana-Maria      | First name cannot contain special characters  |
	| SirName   | O'Conner       | Sir name cannot contain special characters    |
	| City      | London         | City does not belong to the specified country |
	| Country   | France         | City does not belong to the specified country |
	| Title     | Mr             | Title is not valid                            | # 500 Internal server error
	| Email     | @test.com      | Invalid email format                          |
	| Email     | test.com       | Invalid email format                          |
	| Email     | test@test      | Invalid email format                          |
	| Email     | test@@test.com | Invalid email format                          |
	| Email     | .@test.com     | Invalid email format                          | @Issue # 200 OK on first run even email is not valid, and afterthat 409 CONFLICT (duplicate email)
	| City      | Ruse           | City does not belong to the specified country | # Ruse does not exist in database


@Negative
Scenario: Update user with existing email returns validation error
	Given I create a new user via the API
	When I update that user with existing email "admin@automation.com"
	Then the response status code should be 409
	And the response should contain the following error message "Email already in use"


@Negative @Issue
# Actual: API returns 200 OK with a null response body instead of the expected 404 Not Found for a non-existing user ID
Scenario Outline: Update users by id returns the correct error for non-existing user ID
	When I make an Update request to users endpoint with id <id>
	Then the response status code should be 404
	And the response should contain the following error message "User not found"

Examples:
	| id        |
	|         0 |
	| 123456789 |


@Negative
# Actual: API returns an HTML 404 page instead of a JSON error response
Scenario: Update users by id with negative ID value returns error
	When I make an Update request to users endpoint with id -1
	Then the response status code should be 404
	And the response should contain the following error message "Not Found"