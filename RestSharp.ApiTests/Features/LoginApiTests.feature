@LoginApi
Feature: LoginApiTests

Login with valid credentials should be successful. Where non-valid credentials are provided, an appropriate error message is displayed.


Scenario: Login with existing user should return the correct user's detail
	Given I make a post request to login endpoint with valid user credentials
		| Email                | Password |
		| admin@automation.com | pass123  |
	Then the response status code should be 200
	And the login response should contain the following data:
		| Id | FirstName | SirName    | Title | Email                | Password | Country  | City  | IsAdmin |
		|  1 | Admin     | Automation | Mr.   | admin@automation.com | pass123  | Bulgaria | Sofia |       1 |


@Negative
Scenario Outline: Login with invalid credentials should return error
	Given I make a post request to login endpoint with not valid user credentials "<email>" and "<password>"
	Then the response status code should be <status>
	And the response should contain the following error message "<message>"

Examples:
	| email                 | password | status | message                         |
	| admin@automation.com  |   123456 |    401 | Invalid email or password       | # wrong password
	| admin@automation.com  |          |    401 | Invalid email or password       | # empty password
	| admin1@automation.com | pass123  |    401 | Invalid email or password       | # wrong email
	| admin1@automation.com |   123456 |    401 | Invalid email or password       | # both are wrong
	|                       | pass123  |    400 | Email and password are required | # empty email
	|                       |          |    400 | Email and password are required | # both are empty


Scenario: Login with newly created user should return the correct user's detail
	Given I create a new user via the API
	When I login with that user
	Then the response status code should be 200
	And the login response should contain correct user's details


@Negative
Scenario: Login with GET should return error
	Given I make a get request to login endpoint with valid user credentials
		| Email                | Password |
		| admin@automation.com | pass123  |
	Then the response status code should be 405
	And the response should contain the following error message "Method Not Allowed"


@Negative
Scenario Outline: Login with missing fields should return error
	Given I make a post request to login endpoint with body:
		"""
		{ <json> }
		"""
	Then the response status code should be 400
	And the response should contain the following error message "Email and password are required"

Examples:
	| json                            |
	| "password": "pass123"           | # missing email
	| "email": "admin@automation.com" | # missing password
	|                                 | # missing both


@Negative
# Expected status code 400, but actual is 500
Scenario: Login with empty body should return error
	Given I make a post request to login endpoint with empty body
	Then the response status code should be 500
	And the response should contain the following error message "Internal server error"


@Negative
# Expected status code 400, but actual is 500
Scenario: Login with malformed JSON should return error
	Given I make a post request to login endpoint with malformed JSON body
	Then the response status code should be 500
	And the response should contain the following error message "Internal server error"


@Security
Scenario: Login with SQL injection attempt should return error
	Given I make a post request to login endpoint with SQL injection attempt
	Then the response status code should be 401
	And the response should contain the following error message "Invalid email or password"


@Security
Scenario: Login with XSS injection should return error
	Given I make a post request to login endpoint with XSS injection payload
	Then the response status code should be 401
	And the response should contain the following error message "Invalid email or password"


@Security
Scenario: Login with extremely long input should return error
	Given I make a post request to login endpoint with extremely long credentials
	Then the response status code should be 401
	And the response should contain the following error message "Invalid email or password"