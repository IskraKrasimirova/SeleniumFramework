using Microsoft.Extensions.DependencyInjection;
using SeleniumTestFramework.UiTests.Models.UserModels;
using SeleniumTestFramework.UiTests.Pages;
using SeleniumTestFramework.UiTests.Utilities;

namespace SeleniumTestFramework.Tests
{
    [TestFixture(Category = "Register")]
    public class RegisterTests:UiTestBase
    {
        private RegisterPage _registerPage;

        [SetUp]
        public void TestSetup()
        {
            Driver.Navigate().GoToUrl($"{Settings.BaseUrl}register.php");
            _registerPage = TestScope.ServiceProvider.GetRequiredService<RegisterPage>();
        }

        [Test]
        public void RegistrationWith_ValidUserData_LogsUserIn_AndShowsDashboardPage()
        {
            var newUser = UserFactory.CreateValidUser();
            _registerPage.RegisterNewUser(newUser);

            var dashboardPage = TestScope.ServiceProvider.GetRequiredService<DashboardPage>();
            dashboardPage.VerifyIsAtDashboardPage();
            dashboardPage.VerifyUserIsLoggedIn(newUser.Email, $"{newUser.FirstName} {newUser.Surname}", false);
        }

        [Test]
        public void RegistrationWith_EmptyUserData_ShowsErrorMessages()
        {
            var newUser = new RegisterModel("Mr.", "", "", "", "", "", "", false);
            // "Mr." is a default title in dropdown. The title cannot be empty due to the structure of HTML.
            // So, the error message for title will never appear. Issue!
            _registerPage.RegisterNewUser(newUser);
            var firstNameValidationMessage = _registerPage.GetFirstNameValidationMessage();
            var surnameValidationMessage = _registerPage.GetSurnameValidationMessage();
            var emailValidationMessage = _registerPage.GetEmailValidationMessage();
            var passwordValidationMessage = _registerPage.GetPasswordValidationMessage();
            var countryValidationMessage = _registerPage.GetCountryValidationMessage();
            var cityValidationMessage = _registerPage.GetCityValidationMessage();
            var agreementValidationMessage = _registerPage.GetAgreementValidationMessage();

            Assert.Multiple(() =>
            {
                Assert.That(firstNameValidationMessage, Is.EqualTo("Please enter a valid first name (letters only, 2-15 characters)."), "First name validation message is incorrect.");
                Assert.That(surnameValidationMessage, Is.EqualTo("Please enter a valid surname (letters only, 2-15 characters)."), "Surname validation message is incorrect.");
                Assert.That(emailValidationMessage, Is.EqualTo("Please enter a valid email address."), "Email validation message is incorrect.");
                Assert.That(passwordValidationMessage, Is.EqualTo("Password must be at least 6 characters long."), "Password validation message is incorrect.");
                Assert.That(countryValidationMessage, Is.EqualTo("Please enter your country."), "Country validation message is incorrect.");
                Assert.That(cityValidationMessage, Is.EqualTo("Please enter your city."), "City validation message is incorrect.");
                Assert.That(agreementValidationMessage, Is.EqualTo("You must agree to the terms of service."), "Agreement validation message is incorrect.");
            });
        }

        [Test]
        public void RegistrationWith_ExistingEmail_ShowsErrorMessage()
        {
            var newUser = UserFactory.CreateUserWith((RegisterModel u) => u.Set(email: Settings.Email));
            _registerPage.RegisterNewUser(newUser);

            _registerPage.VerifyPasswordInputIsEmpty();
            _registerPage.VerifyGlobalAlertMessage("User with such email already exists");
        }
        
        // There is a relationship between the Country and City fields.
        // The city must belong to the specified country. But the city field does not have
        // a dropdown or autocomplete to help the user select a valid city for the chosen country.
        [Test]
        [Category("BackendIssue")]
        public void RegistrationWith_NotValidCityForCountry_ShowsErrorMessage()
        {
            var newUser = UserFactory.CreateUserWith((RegisterModel u) => u.Set(city: "New York"));
            _registerPage.RegisterNewUser(newUser);

            _registerPage.VerifyPasswordInputIsEmpty();

            var errorMessage = _registerPage.GetGlobalAlertMessage();

            Assert.That(errorMessage, Is.EqualTo("City does not belong to the specified country"), "Expected city-country validation message was not shown.");
        }

        // Backend has a hidden 15-character limit for city names. 
        // When the city exceeds 15 characters, the server returns a PHP warning instead of 
        // the expected validation message. The frontend displays the warning directly, 
        // so the correct message never appears.
        // For example, "Gorno Draglishte" has 16 characters, and is a valid city in Bulgaria. But the test fails with error message: Warning: Array to string conversion in /var/www/html/register.php on line 122 Array
        // Another examples: "Sofia City Center" has 17 characters and the same issue occurs.
        // "InvalidCityName123" has 18 characters and the same issue occurs.
        // "Novo Selo Vidin" has 15 characters and works fine.
        // Invalid city name with 1 character has the same issue.
        // So, there is a backend validation for city length between 2 and 15 characters, but it is not shown with clear message in UI.
        // The same hidden 2–15 character length validation applies to the Country field. 
        // Values shorter than 2 or longer than 15 characters cause the backend to return 
        // a PHP warning instead of a proper validation message, and the UI does not show 
        // a clear error to the user.
        [Test]
        [Category("BackendIssue")]
        public void RegistrationWith_TooLongCountryNameAndValidCityForCountry_ShowsErrorMessage()
        {
            var newUser = UserFactory.CreateUserWith((RegisterModel u) => 
            u.Set(country: "Bosnia and Herzegovina", city: "Tuzla"));
            _registerPage.RegisterNewUser(newUser);

            _registerPage.VerifyPasswordInputIsEmpty();

            var errorMessage = _registerPage.GetGlobalAlertMessage();
            Console.WriteLine($"ERROR: {errorMessage}");

            Assert.That(errorMessage, Does.Contain("Warning"), "Backend returned a PHP warning instead of a proper validation message.");
        }

        [Test]
        [Category("BackendIssue")]
        public void RegistrationWith_TooLongCityNameAndValidCityForCountry_ShowsErrorMessage()
        {
            var newUser = UserFactory.CreateUserWith((RegisterModel u) => u.Set(city: "Gorno Draglishte"));
            _registerPage.RegisterNewUser(newUser);

            _registerPage.VerifyPasswordInputIsEmpty();

            var errorMessage = _registerPage.GetGlobalAlertMessage();
            Console.WriteLine($"ERROR: {errorMessage}");

            Assert.That(errorMessage, Does.Contain("Warning"), "Backend returned a PHP warning instead of a proper validation message.");
        }


        [Test]
        [Category("BackendIssue")]
        public void RegistrationWith_ValidCityForCountry_WithUnicodeCharacters_ShowsBackendWarning()
        {
            var newUser = UserFactory.CreateUserWith((RegisterModel u) => u.Set(country: "Côte d'Ivoire", city: "Nuku'alofa"));
            _registerPage.RegisterNewUser(newUser);

            _registerPage.VerifyPasswordInputIsEmpty();

            var errorMessage = _registerPage.GetGlobalAlertMessage();
            Console.WriteLine($"ERROR: {errorMessage}");

            Assert.That(errorMessage, Does.Contain("Warning"), "Backend returned a PHP warning instead of a proper validation message.");
        }

        // These are valid country and city names that include special characters, but the error message is not proper.
        [Test]
        [TestCaseSource(nameof(CityOrCountryWithSpecialCharacters))]
        [Category("BackendIssue")]
        public void RegistrationWith_ValidCityForCountry_WithSpecialCharacters_ShowsErrorMessage(string testedCase, string country, string city)
        {
            var newUser = UserFactory.CreateUserWith((RegisterModel u) => u.Set(country: country, city: city));
            _registerPage.RegisterNewUser(newUser);

            _registerPage.VerifyPasswordInputIsEmpty();

            var errorMessage = _registerPage.GetGlobalAlertMessage();
            Console.WriteLine($"ERROR: {errorMessage}");

            Assert.That(errorMessage, Is.Not.Null.And.Not.Empty, "Expected backend to return an error message, but got nothing.");
            Assert.That(errorMessage, Is.EqualTo("City does not belong to the specified country"), "Expected validation message was not shown.");
        }

        private static IEnumerable<TestCaseData> CityOrCountryWithSpecialCharacters()
        {
            yield return new TestCaseData("Country name with symbols", "Guinea-Bissau", "Bafata");
            yield return new TestCaseData("City name with symbols", "Australia", "O'Connor");
        }
    }
}