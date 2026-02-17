using Microsoft.Extensions.DependencyInjection;
using Selenium.UiTests.Pages;
using Selenium.UiTests.Tests;
using Selenium.UiTests.Utilities;

namespace Selenium.Tests
{
    [TestFixture(Category = "Users")]
    public class UsersTests: UiTestBase
    {
        private LoginPage _loginPage;
        private RegisterPage _registerPage; 
        private DashboardPage _dashboardPage;

        [SetUp]
        public void TestSetup()
        {
            _loginPage = TestScope.ServiceProvider.GetRequiredService<LoginPage>();
            _registerPage = TestScope.ServiceProvider.GetRequiredService<RegisterPage>();
            _dashboardPage = TestScope.ServiceProvider.GetRequiredService<DashboardPage>();

            Driver.Navigate().GoToUrl(Settings.BaseUrl);
        }

        [Test]
        public void UserCanRegister_AndAdminCanDeleteUser()
        {
            _loginPage.GoToRegisterPage();
            _registerPage.VerifyIsAtRegisterPage();

            var newUser = UserFactory.CreateValidUser();

            _registerPage.RegisterNewUser(newUser);

            _dashboardPage.VerifyIsAtDashboardPage();
            _dashboardPage.VerifyUserIsLoggedIn(newUser.Email, $"{newUser.FirstName} {newUser.Surname}", false);

            _dashboardPage.Logout();
            _loginPage.VerifyIsAtLoginPage();

            _loginPage.LoginWith(Settings.Email, Settings.Password);
            _dashboardPage.VerifyIsAtDashboardPage();
            _dashboardPage.VerifyUserIsLoggedIn(Settings.Email, Settings.Username, true);

            var usersPage = _dashboardPage.GoToUsersPage();
            usersPage.VerifyIsAtUsersPage(true);
            usersPage.VerifyUserExists(newUser.Email);

            usersPage.DeleteUser(newUser.Email);
            usersPage.VerifyUserDoesNotExist(newUser.Email);

            _dashboardPage.Logout();
            _loginPage.VerifyIsAtLoginPage();

            _loginPage.LoginWith(newUser.Email, newUser.Password);

            _loginPage.VerifyPasswordInputIsEmpty();
            _loginPage.VerifyErrorMessageIsDisplayed("Invalid email or password");
        }
    }
}
