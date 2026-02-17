using Common.Models;
using Common.Models.Factories;
using Common.Utilities;
using OpenQA.Selenium;
using Reqnroll;
using RestSharp.ApiTests.Apis;
using RestSharp.ApiTests.Models.Dtos;
using RestSharp.ApiTests.Models.Factories;
using Selenium.UiTests.DatabaseOperations.Operations;
using Selenium.UiTests.Pages;
using Selenium.UiTests.Utilities;
using Selenium.UiTests.Utilities.Constants;

namespace Selenium.UiTests.Steps
{
    [Binding]
    public class RegisterSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly RegisterPage _registerPage;
        private readonly DashboardPage _dashboardPage;
        private readonly LoginPage _loginPage;
        private readonly UserOperations _userOperations;
        private readonly IUserFactory _userFactory;
        private readonly UsersApi _usersApi;
        private readonly IApiUserFactory _apiUserFactory;

        public RegisterSteps(IUserFactory userFactory, ScenarioContext scenarioContext, RegisterPage _registerPage, DashboardPage dashboardPage, LoginPage loginPage, UserOperations userOperations, UsersApi usersApi, IApiUserFactory apiUserFactory)
        {
            _userFactory = userFactory;
            _scenarioContext = scenarioContext;
            this._registerPage = _registerPage;
            _dashboardPage = dashboardPage;
            _loginPage = loginPage;
            _userOperations = userOperations;
            _usersApi = usersApi;
            _apiUserFactory = apiUserFactory;
        }

        [Given("I register a new user")]
        public void GivenIRegisterANewUser()
        {
            _loginPage.GoToRegisterPage();

            _registerPage.VerifyIsAtRegisterPage();
            var newUser = UserFactory.CreateValidUser();
            _registerPage.RegisterNewUser(newUser);
            _scenarioContext.Add(ContextConstants.RegisteredUser, newUser);

            _dashboardPage.VerifyIsAtDashboardPage();
            _dashboardPage.VerifyUserIsLoggedIn(newUser.Email, $"{newUser.FirstName} {newUser.Surname}", false);

            _dashboardPage.Logout();
        }

        [Given("I register new user with valid details")]
        public void GivenIRegisterNewUserWithValidDetails()
        {
            _loginPage.VerifyIsAtLoginPage();

            _loginPage.GoToRegisterPage();
            _registerPage.VerifyIsAtRegisterPage();

            var newUser = _userFactory.CreateDefault<UserModel>();
            _registerPage.RegisterNewUser(newUser);
            _scenarioContext.Add(ContextConstants.NewRegisteredUser, newUser);

            Retry.Until(() =>
            {
                var doesUserExist = _userOperations.CheckIfUserExistsByEmail(newUser.Email);
                if (doesUserExist == false)
                    throw new RetryException("Registerd User is not found in the database.");
            }, [new StaleElementReferenceException()]);
        }

        [Given("I create a user successfully")]
        public void GivenICreateAUserSuccessfully()
        {
            var newUser = _apiUserFactory.CreateDefault();
            var userResponse = _usersApi.CreateUser<UserDto>(newUser);

            Assert.That(userResponse.Data, Is.Not.Null);

            var user = userResponse.Data;

            var registeredUser = _userFactory.CreateCustom<UserModel>(
                title: user.Title,
                firstName: user.FirstName,
                surname: user.SirName,
                country: user.Country,
                city: user.City,
                email: user.Email,
                password: user.Password
            );

            _scenarioContext[ContextConstants.NewRegisteredUser] = registeredUser;
        }

        [When("I verify that the registration form is displayed")]
        public void WhenIVerifyThatTheRegistrationFormIsDisplayed()
        {
            _registerPage.VerifyIsAtRegisterPage();
        }

        [When("I register a new user with valid details")]
        public void WhenIRegisterANewUserWithValidDetails()
        {
            var newUser = UserFactory.CreateValidUser();
            _registerPage.RegisterNewUser(newUser);
            _scenarioContext.Add(ContextConstants.RegisteredUser, newUser);
        }
    }
}
