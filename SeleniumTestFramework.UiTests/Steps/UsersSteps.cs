using Reqnroll;
using SeleniumTestFramework.UiTests.Models;
using SeleniumTestFramework.UiTests.Models.UserModels;
using SeleniumTestFramework.UiTests.Pages;
using SeleniumTestFramework.UiTests.Utilities;
using SeleniumTestFramework.UiTests.Utilities.Constants;

namespace SeleniumTestFramework.UiTests.Steps
{
    [Binding]
    public class UsersSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly UsersPage _usersPage;

        public UsersSteps(ScenarioContext scenarioContext, UsersPage usersPage)
        {
            _scenarioContext = scenarioContext;
            _usersPage = usersPage;
        }

        [Given("I open the Add New User form")]
        public void GivenIOpenTheAddNewUserForm()
        {
            var addUserModal = _usersPage.OpenAddUserModal();
            addUserModal.VerifyIsAtAddUserModal();
        }

        [When("I delete the created user")]
        public void WhenIDeleteTheCreatedUser()
        {
            _usersPage.VerifyIsAtUsersPage(true);
            var user = _scenarioContext.Get<RegisterModel>(ContextConstants.RegisteredUser);
            _usersPage.VerifyUserExists(user.Email);

            _usersPage.DeleteUser(user.Email);
            _usersPage.VerifyUserDoesNotExist(user.Email);
        }

        [When("I add a new user with valid details")]
        public void WhenIAddANewUserWithValidDetails()
        {
            var modal = _usersPage.OpenAddUserModal();
            modal.VerifyIsAtAddUserModal();

            var newUser = UserFactory.CreateValidCommonUser();
            modal.AddUser(newUser);

            _scenarioContext.Add(ContextConstants.AddedUser, newUser);

            modal.VerifyModalIsClosed();

            _usersPage.VerifyIsAtUsersPage(true);
            _usersPage.VerifyUserExists(newUser.Email);
        }

        [Then("the new user should be present in the users list")]
        public void ThenTheNewUserShouldBePresentInTheUsersList()
        {
            _usersPage.VerifyIsAtUsersPage(true);
            var newUser = _scenarioContext.Get<RegisterModel>(ContextConstants.RegisteredUser);
            _usersPage.VerifyUserExists(newUser.Email);
        }

        [Then("the user should no longer be present in the users list")]
        public void ThenTheUserShouldNoLongerBePresentInTheUsersList()
        {
            //var newUser = (RegisterModel)_scenarioContext["RegisteredUser"];
            var newUser = _scenarioContext.Get<RegisterModel>(ContextConstants.RegisteredUser);
            _usersPage.VerifyUserDoesNotExist(newUser.Email);
        }

        [Then("I should see the created user in the users list")]
        public void ThenIShouldSeeTheCreatedUserInTheUsersList()
        {
            _usersPage.VerifyIsAtUsersPage(true);
            var expectedUser = _scenarioContext.Get<UserModel>(ContextConstants.NewRegisteredUser);
            _usersPage.VerifyUserRowMatches(expectedUser);
        }
    }
}
