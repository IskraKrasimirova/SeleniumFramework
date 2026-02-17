Feature: AddUserValidationTests

The system should not allow the administrator to add new users with invalid details. Appropriate error messages and validations should be displayed when invalid data is provided.

Background:
	Given I navigate to the main page
	And I verify that the login form is displayed
	And I login with admin credentials
	And I navigate to the users page
	And I open the Add New User form

@Users @Negative
Scenario: Duplicate email is rejected
	When I try to add a new user with an email that already exists
	Then I should see an error message "User with such email already exists"

@Users @Negative
Scenario: All required fields empty show all validation messages
	When I try to add a new user with all fields empty
	Then I should see the following validation messages:
		| field     | message                             |
		| title     | Please select a title.              |
		| firstName | Please enter a first name.          |
		| surname   | Please enter a surname.             |
		| country   | Please select a country.            |
		| city      | Please enter a city.                |
		| email     | Please enter a valid email address. |
		| password  | Please enter a password.            |

@Users @Negative
# Testing every required field separately
Scenario Outline: Required field validation
	When I try to add a new user with missing "<field>"
	Then I should see a validation message for "<field>" with text "<message>"

Examples:
	| field     | message                             |
	| title     | Please select a title.              |
	| firstName | Please enter a first name.          |
	| surname   | Please enter a surname.             |
	| country   | Please select a country.            |
	| city      | Please enter a city.                |
	| email     | Please enter a valid email address. |
	| password  | Please enter a password.            |

#@Users @Negative
#Scenario: Invalid email format is rejected
#	When I try to add a new user with an invalid email format
#	Then I should see a validation message for "email" with text "Please enter a valid email address."

@Users @Negative
Scenario Outline: Name fields exceeding maximum length are rejected
	When I try to add a new user with "<field>" longer than allowed
	Then I should see an error message "<message>"

Examples:
	| field     | message                                        |
	| firstName | First Name must be between 2 and 15 characters |
	| surname   | Sir Name must be between 2 and 15 characters   |

@Users @Negative
Scenario: City does not belong to the selected country
	When I try to add a new user with country "Bulgaria" and city "London"
	Then I should see an error message "City does not belong to the specified country"

@Users @Negative @issue
# Backend incorrectly returns both errors: length + mismatch
Scenario Outline: City name exceeding maximum length is rejected with incorrect message for country-city mismatch
	When I try to add a new user with country "<country>" and city "<city>"
	Then I should see an error message "City must be between 2 and 15 charactersCity does not belong to the specified country"

Examples:
	| country  | city                |
	| Bulgaria | Gorna Oryahovitsa   |
	| UK       | Newcastle upon Tyne |


@Users @Negative @issue
Scenario Outline: City name with special characters is rejected with incorrect message for country-city mismatch
	When I try to add a new user with country "<country>" and city "<city>"
	Then I should see an error message "City does not belong to the specified country"

Examples:
	| country | city         |
	| Germany | Bad-Harzburg |
	| UK      | King’s Lynn  |
	| Romania | Brașov       |
	| Greece  | Pátra        |
	| Germany | Köln         |


@Users @Negative
Scenario Outline: Invalid email format is rejected
	When I try to add a new user with email "<email>"
	Then I should see a validation message for "email" with text "Please enter a valid email address."

Examples:
	| email                |
	| testemail.com        | # missing @
	| @domain.com          | # missing local part
	| user@                | # missing domain
	| user test@domain.com | # with space
	| user.name@@a.com     | # double @
	| user@do_main.com     | # invalid domain char


@Users @Negative @issue
Scenario Outline: Invalid email format is validated by the system
	When I try to add a new user with email "<email>"
	Then I should see an error message "Invalid email format"

Examples:
	| email            |
	| user@domain      | # missing TLD
	| user!@domain.com | # with special char
	| a+1@a.com        | # with plus sign but the system restricts it


@Users @Negative @issue @ignore
# The system incorrectly accepts these invalid emails and does not show any error message
# These test cases are ignored until the system is fixed
Scenario Outline: Invalid email format is not validated by the system
	When I try to add a new user with email "<email>"
	Then I should see a validation message for "email" with text "Please enter a valid email address."

Examples:
	| email            |
	| .testuser@a.com  | # starts with dot
	| test..user@a.com | # double dot
