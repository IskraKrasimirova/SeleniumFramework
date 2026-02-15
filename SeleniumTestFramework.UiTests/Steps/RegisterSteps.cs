using Reqnroll;
using SeleniumTestFramework.ApiTests.Apis;
using SeleniumTestFramework.ApiTests.Models.Dtos;
using SeleniumTestFramework.UiTests.DatabaseOperations.Operations;
using SeleniumTestFramework.UiTests.Models.Factories;
using SeleniumTestFramework.UiTests.Pages;
using SeleniumTestFramework.UiTests.Utilities;
using SeleniumTestFramework.UiTests.Utilities.Constants;

namespace SeleniumTestFramework.Steps
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
        private readonly ApiTests.Models.Factories.IUserFactory _apiUserFactory;

        public RegisterSteps(IUserFactory userFactory, ScenarioContext scenarioContext, RegisterPage _registerPage, DashboardPage dashboardPage, LoginPage loginPage, UserOperations userOperations, UsersApi usersApi, ApiTests.Models.Factories.IUserFactory apiUserFactory)
        {
            this._userFactory = userFactory;
            this._scenarioContext = scenarioContext;
            this._registerPage = _registerPage;
            this._dashboardPage = dashboardPage;
            this._loginPage = loginPage;
            this._userOperations = userOperations;
            this._usersApi = usersApi;
            this._apiUserFactory = apiUserFactory;
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

            var newUser = _userFactory.CreateDefault();
            _registerPage.RegisterNewUser(newUser);
            _scenarioContext.Add(ContextConstants.NewRegisteredUser, newUser);

            Retry.Until(() =>
            {
                var doesUserExist = _userOperations.CheckIfUserExistsByEmail(newUser.Email);
                if (doesUserExist == false)
                    throw new RetryException("Registerd User is not found in the database.");
            });
        }

        [Given("I create a user successfully")]
        public void GivenICreateAUserSuccessfully()
        {
            var newUser = _apiUserFactory.CreateDefault();
            var userResponse = _usersApi.CreateUser<UserDto>(newUser);

            Assert.That(userResponse.Data, Is.Not.Null);

            var user = userResponse.Data;

            var registeredUser = _userFactory.CreateCustom(
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
