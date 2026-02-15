using Bogus;
using Microsoft.Extensions.DependencyInjection;
using SeleniumTestFramework.UiTests.Pages;
using SeleniumTestFramework.UiTests.Utilities;
using SeleniumTestFramework.UiTests.Utilities.Extensions;

namespace SeleniumTestFramework.Tests
{
    [TestFixture(Category ="Login")]
    public class LoginTests: UiTestBase
    {
        private LoginPage _loginPage;
        private DashboardPage _dashboardPage;

        [SetUp]
        public void TestSetup()
        {
            _loginPage = TestScope.ServiceProvider.GetRequiredService<LoginPage>();
            _dashboardPage = TestScope.ServiceProvider.GetRequiredService<DashboardPage>();
            Driver.Navigate().GoToUrl(Settings.BaseUrl);
        }

        [Test]
        [Category("FromLiveSession")]
        public void LoginWith_ExistingUser_ShouldShowTheDashboard()
        {
            _loginPage.VerifyIsAtLoginPage();
            _loginPage.LoginWith(Settings.Email, Settings.Password);

            _dashboardPage.VerifyLoggedUserEmailIs(Settings.Email);
            _dashboardPage.VerifyUsernameIs(Settings.Username);
        }

        [Test]
        [TestCaseSource(nameof(ValidLoginData))]
        public void LoginWith_ValidUserCredentials_ShouldShowTheDashboard(string email, string password, string username, bool isAdmin)
        {
            // Verify we are on the correct page BEFORE interacting
            _loginPage.VerifyIsAtLoginPage();

            _loginPage.LoginWith(email, password);
            Driver.WaitUntilUrlContains("/index");

            _dashboardPage.VerifyIsAtDashboardPage();
            _dashboardPage.VerifyUserIsLoggedIn(email, username, isAdmin);
        }

        private static IEnumerable<TestCaseData> ValidLoginData()
        {
            var settingsModel = ConfigurationManager.Instance.SettingsModel;

            yield return new TestCaseData(settingsModel.Email, settingsModel.Password, settingsModel.Username, true);
            yield return new TestCaseData("idimitrov@automation.com", "pass123", "Ivan Dimotrov", false);
        }

        [Test]
        [TestCaseSource(nameof(NotValidLoginData))]
        public void LoginWith_NotValidUserCredentials_ShowsValidationMessage(string testedCase, string email, string password)
        {
            // Verify we are on the correct page BEFORE interacting
            _loginPage.VerifyIsAtLoginPage();

            _loginPage.LoginWith(email, password);

            Retry.Until(() =>
            {
                if (!_loginPage.IsPasswordInputEmpty())
                    throw new RetryException("Password input is not empty yet.");
            });

            _loginPage.VerifyPasswordInputIsEmpty();
            _loginPage.VerifyErrorMessageIsDisplayed("Invalid email or password"); 
        }

        private static IEnumerable<TestCaseData> NotValidLoginData()
        {
            var settingsModel = ConfigurationManager.Instance.SettingsModel;

            yield return new TestCaseData("Wrong password", settingsModel.Email, "password");
            yield return new TestCaseData("Wrong email", "admin@admin.com", settingsModel.Password);
            yield return new TestCaseData("Wrong email and password", "a@a.com", "wrongpassword");
        }

        [Test]
        [Category("FromLiveSession")]
        public void LoginWith_NonExistingUser_ShowsValidationMessage()
        {
            var faker = new Faker();
            _loginPage.LoginWith(faker.Internet.Email(), faker.Internet.Password());

            Retry.Until(() =>
            {
                if (!_loginPage.IsPasswordInputEmpty())
                    throw new RetryException("Password input is not empty yet.");
            });

            _loginPage.VerifyPasswordInputIsEmpty();
            _loginPage.VerifyErrorMessageIsDisplayed("Invalid email or password");
        }

        [Test]
        [TestCaseSource(nameof(InvalidEmailFormat))]
        public void LoginWith_InvalidEmailFormat_ShowsBrowserValidationMessage(string email, string password, string message)
        {
            // Verify we are on the correct page BEFORE interacting
            _loginPage.VerifyIsAtLoginPage();

            _loginPage.LoginWith(email, password);

            var validationMessage = _loginPage.GetEmailBrowserValidationMessage();

            Assert.That(validationMessage, Does.Contain(message));
        }

        private static IEnumerable<TestCaseData> InvalidEmailFormat()
        {
            yield return new TestCaseData("abc", "pass123", "missing an '@'");
            yield return new TestCaseData("abc@", "pass123", "incomplete");
            yield return new TestCaseData("abc abc@test.com", "pass123", "should not contain the symbol ' '");
            yield return new TestCaseData("@test", "pass123", "incomplete");
            yield return new TestCaseData("@test.com", "pass123", "incomplete");
            yield return new TestCaseData("abc@@test.com", "", "should not contain the symbol '@'");
            yield return new TestCaseData("abc@.test.com", "", "a wrong position");
            yield return new TestCaseData("abc@test .com", "", "should not contain the symbol ' '");
            yield return new TestCaseData("", "", "fill out this field");
        }

        [Test]
        public void LoginWith_ShortPassword_ShowsValidationMessage()
        {
            // Verify we are on the correct page BEFORE interacting
            _loginPage.VerifyIsAtLoginPage();

            _loginPage.LoginWith("admin@automation.com", "123");

            var message = _loginPage.GetPasswordValidationMessage();

            Assert.That(message, Is.EqualTo("Password must be at least 6 characters"));
        }

        [Test]
        public void LoginWith_EmptyPassword_ShowsBrowserValidationMessage()
        {
            // Verify we are on the correct page BEFORE interacting
            _loginPage.VerifyIsAtLoginPage();

            _loginPage.LoginWith("admin@automation.com", "");

            var message = _loginPage.GetPasswordBrowserValidationMessage();

            Assert.That(message, Is.EqualTo("Please fill out this field."));
        }
    }
}