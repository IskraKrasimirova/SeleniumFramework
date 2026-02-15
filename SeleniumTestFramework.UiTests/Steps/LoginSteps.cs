using OpenQA.Selenium;
using Reqnroll;
using SeleniumTestFramework.UiTests.Models;
using SeleniumTestFramework.UiTests.Models.UserModels;
using SeleniumTestFramework.UiTests.Pages;
using SeleniumTestFramework.UiTests.Utilities;
using SeleniumTestFramework.UiTests.Utilities.Constants;

namespace SeleniumTestFramework.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPage _loginPage;
        private readonly SettingsModel _settingsModel;

        public LoginSteps(IWebDriver driver, ScenarioContext scenarioContex, LoginPage loginPage, SettingsModel model)
        {
            this._driver = driver;
            this._scenarioContext = scenarioContex;
            this._loginPage = loginPage;
            this._settingsModel = model;
        }

        [Given("I navigate to the main page")]
        public void GivenINavigateToTheMainPage()
        {
            _driver.Navigate().GoToUrl(_settingsModel.BaseUrl);
        }

        [Given("I verify that the login form is displayed")]
        public void GivenIVerifyThatTheLoginFormIsDisplayed()
        {
            _loginPage.VerifyIsAtLoginPage();
        }

        [Given("I login with admin credentials")]
        public void GivenILoginWithAdminCredentials()
        {
            _loginPage.VerifyIsAtLoginPage();
            _loginPage.LoginWith(_settingsModel.Email, _settingsModel.Password);
        }

        [When("I login with valid credentials")]
        public void WhenILoginWithValidCredentials()
        {
            _loginPage.LoginWith(_settingsModel.Email, _settingsModel.Password);
        }

        [When("I login with invalid credentials")]
        public void WhenILoginWithInvalidCredentials()
        {
            _loginPage.LoginWith("notexistinguser@gmail.com", _settingsModel.Password);
        }

        [When("I login with {string} and {string}")]
        public void WhenILoginWithAnd(string email, string password)
        {
            if (email == "readFromSettings")
            {
                WhenILoginWithValidCredentials();
            }
            else
            {
                _loginPage.LoginWith(email, password);
            }
        }

        [When("I navigate to the registration page")]
        public void WhenINavigateToTheRegistrationPage()
        {
            _loginPage.GoToRegisterPage();
        }

        [When("I login with admin credentials")]
        public void WhenILoginWithAdminCredentials()
        {
            _loginPage.LoginWith(_settingsModel.Email, _settingsModel.Password);
        }

        [When("I try to login with the deleted user's credentials")]
        public void WhenITryToLoginWithTheDeletedUsersCredentials()
        {
            var deletedUser = _scenarioContext.Get<RegisterModel>(ContextConstants.RegisteredUser);
            _loginPage.LoginWith(deletedUser.Email, deletedUser.Password);
        }

        [Then("I should still be on the login page")]
        public void ThenIShouldStillBeOnTheLoginPage()
        {
            Assert.That(_driver.Url, Is.EqualTo(_settingsModel.BaseUrl + "login.php"));
        }

        [Then("I should see an error message with the following text {string}")]
        public void ThenIShouldSeeAnErrorMessageWithTheFollowingText(string errorText)
        {
            Retry.Until(() =>
            {
                if (!_loginPage.IsPasswordInputEmpty())
                    throw new RetryException("Password input is not empty yet.");
            });

            _loginPage.VerifyPasswordInputIsEmpty();
            _loginPage.VerifyErrorMessageIsDisplayed(errorText);
        }

        [Then("I login with the deleted user's credentials")]
        public void ThenILoginWithTheDeletedUsersCredentials()
        {
            _loginPage.VerifyIsAtLoginPage();
            var deletedUser = _scenarioContext.Get<RegisterModel>(ContextConstants.RegisteredUser);
            _loginPage.LoginWith(deletedUser.Email, deletedUser.Password);
        }

        [Then("I login with the new user's credentials")]
        public void ThenILoginWithTheNewUsersCredentials()
        {
            _loginPage.VerifyIsAtLoginPage();
            var addedUser = _scenarioContext.Get<AddUserModel>(ContextConstants.AddedUser);
            _loginPage.LoginWith(addedUser.Email, addedUser.Password);
        }

    }
}
