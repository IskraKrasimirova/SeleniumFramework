using Common.Models.SettingsModels;
using Reqnroll;
using Selenium.UiTests.Models;
using Selenium.UiTests.Models.UserModels;
using Selenium.UiTests.Pages;
using Selenium.UiTests.Utilities.Constants;

namespace Selenium.UiTests.Steps
{
    [Binding]
    public class DashboardSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private DashboardPage _dashboardPage;
        private readonly UiSettingsModel _settingsModel;

        public DashboardSteps(ScenarioContext scenarioContext, DashboardPage dashboardPage, UiSettingsModel model)
        {
            _scenarioContext = scenarioContext;
            _dashboardPage = dashboardPage;
            _settingsModel = model;
        }

        [Given("the user can see the dashboard with its data")]
        public void GivenTheUserCanSeeTheDashboardWithItsData()
        {
            var newUser = _scenarioContext.Get<RegisterModel>(ContextConstants.RegisteredUser);
            _dashboardPage.VerifyIsAtDashboardPage();
            _dashboardPage.VerifyUserIsLoggedIn(newUser.Email, $"{newUser.FirstName} {newUser.Surname}", false);
        }

        [Given("the user should be able to logout successfully")]
        public void GivenTheUserShouldBeAbleToLogoutSuccessfully()
        {
            _dashboardPage.Logout();
        }

        [Given("I navigate to the users page")]
        public void GivenINavigateToTheUsersPage()
        {
            _dashboardPage.VerifyIsAtDashboardPage();
            _dashboardPage.VerifyUserIsLoggedIn(_settingsModel.Email, _settingsModel.Username, true);
            _dashboardPage.GoToUsersPage();
        }

        [Given("I navigate to the search page")]
        public void GivenINavigateToTheSearchPage()
        {
            _dashboardPage.VerifyIsAtDashboardPage();
            _dashboardPage.VerifyUserIsLoggedIn(_settingsModel.Email, _settingsModel.Username, true);
            _dashboardPage.GoToSearchPage();
        }

        [When("I verify the dashboard shows admin details")]
        public void WhenIVerifyTheDashboardShowsAdminDetails()
        {
            _dashboardPage.VerifyIsAtDashboardPage();
            _dashboardPage.VerifyUserIsLoggedIn(_settingsModel.Email, _settingsModel.Username, true);
        }

        [When("I navigate to the users page")]
        public void WhenINavigateToTheUsersPage()
        {
            _dashboardPage.GoToUsersPage();
        }

        [When("I log out successefuly")]
        public void WhenILogOutSuccessefuly()
        {
            _dashboardPage.Logout();
        }

        [When("the administrator logs out successfully")]
        public void WhenTheAdministratorLogsOutSuccessfully()
        {
            _dashboardPage.Logout();
        }

        [Then("I should see the logged user in the main header")]
        public void ThenIShouldSeeTheLoggedUserInTheMainHeader()
        {
            _dashboardPage.VerifyLoggedUserEmailIs(_settingsModel.Email);
            _dashboardPage.VerifyUsernameIs(_settingsModel.Username);
        }

        [Then("I should see the logged user {string} in the navbar dropdown")]
        public void ThenIShouldSeeTheLoggedUserInTheNavbarDropdown(string expectedEmail)
        {
            if (expectedEmail == "readFromSettings")
            {
                expectedEmail = _settingsModel.Email;
            }

            _dashboardPage.VerifyLoggedUserEmailIs(expectedEmail);
        }

        [Then("I should be able to logout successfully")]
        public void ThenIShouldBeAbleToLogoutSuccessfully()
        {
            _dashboardPage.Logout();
        }

        [Then("I should see the dashboard of the user")]
        public void ThenIShouldSeeTheDashboardOfTheUser()
        {
            var user = _scenarioContext.Get<RegisterModel>(ContextConstants.RegisteredUser);
            _dashboardPage.VerifyIsAtDashboardPage();
            _dashboardPage.VerifyUserIsLoggedIn(user.Email, $"{user.FirstName} {user.Surname}", false);
        }

        [Then("I should see the dashboard of the added user")]
        public void ThenIShouldSeeTheDashboardOfTheAddedUser()
        {
            var user = _scenarioContext.Get<AddUserModel>(ContextConstants.AddedUser);
            _dashboardPage.VerifyIsAtDashboardPage();
            _dashboardPage.VerifyUserIsLoggedIn(user.Email, $"{user.FirstName} {user.Surname}", false);
        }

        [Then("I should see the created user is logged successfully")]
        public void ThenIShouldSeeTheCreatedUserIsLoggedSuccessfully()
        {
            var registeredUser = _scenarioContext.Get<UserModel>(ContextConstants.NewRegisteredUser);
            _dashboardPage.VerifyLoggedUserEmailIs(registeredUser.Email);
            _dashboardPage.VerifyUsernameIs($"{registeredUser.FirstName} {registeredUser.Surname}");
        }
    }
}
