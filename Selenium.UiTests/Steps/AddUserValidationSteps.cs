using Common.Models.SettingsModels;
using Reqnroll;
using Selenium.UiTests.Models.UserModels;
using Selenium.UiTests.Pages;
using Selenium.UiTests.Utilities;

namespace Selenium.UiTests.Steps
{
    [Binding]
    public class AddUserValidationSteps
    {
        private readonly AddUserModalPage _addUserModal;
        private readonly UiSettingsModel _settingsModel;

        public AddUserValidationSteps(AddUserModalPage addUserModal, UiSettingsModel settingsModel)
        {
            _addUserModal = addUserModal;
            _settingsModel = settingsModel;
        }

        [When("I try to add a new user with an email that already exists")]
        public void WhenITryToAddANewUserWithAnEmailThatAlreadyExists()
        {
            AddUserModel newUser = UserFactory.CreateUserWith((AddUserModel u) => 
            u.Set(email: _settingsModel.Email));
           _addUserModal.AddUser(newUser);
        }

        [When("I try to add a new user with all fields empty")]
        public void WhenITryToAddANewUserWithAllFieldsEmpty()
        {
            var newUser = new AddUserModel("", "", "", "", "", "", "", false);
            _addUserModal.AddUser(newUser);
        }

        [When("I try to add a new user with missing {string}")]
        public void WhenITryToAddANewUserWithMissing(string field)
        {
            var newUser = UserFactory.CreateUserWith((AddUserModel u) =>
            {
                switch (field) 
                { 
                    case "title": u.Set(title: ""); break; 
                    case "firstName": u.Set(firstName: ""); break; 
                    case "surname": u.Set(surname: ""); break; 
                    case "country": u.Set(country: ""); break; 
                    case "city": u.Set(city: ""); break;
                    case "email": u.Set(email: ""); break;
                    case "password": u.Set(password: ""); break;
                    default: 
                        throw new ArgumentException($"Unknown field: {field}"); }
            });

            _addUserModal.AddUser(newUser);
        }

        [When("I try to add a new user with {string} longer than allowed")]
        public void WhenITryToAddANewUserWithLongerThanAllowed(string field)
        {
            var longName = new string('A', 16);

            var newUser = UserFactory.CreateUserWith((AddUserModel u) =>
            {
                switch (field)
                {
                    case "firstName": u.Set(firstName: longName); break;
                    case "surname": u.Set(surname: longName); break;
                    default:
                        throw new ArgumentException($"Unknown field: {field}");
                }
            });

            _addUserModal.AddUser(newUser);
        }

        [When("I try to add a new user with country {string} and city {string}")]
        public void WhenITryToAddANewUserWithCountryAndCity(string country, string city)
        {
            var newUser = UserFactory.CreateUserWith((AddUserModel u) =>
            {
                u.Set(country: country);
                u.Set(city: city);
            });

            _addUserModal.AddUser(newUser);
        }

        [When("I try to add a new user with email {string}")]
        public void WhenITryToAddANewUserWithEmail(string email)
        {
            var newUser = UserFactory.CreateUserWith((AddUserModel u) => u.Set(email: email));

            _addUserModal.AddUser(newUser);
        }

        [Then("I should see an error message {string}")]
        public void ThenIShouldSeeAnErrorMessage(string errorMessage)
        {
            _addUserModal.VerifyFormAlertMessage(errorMessage);
        }

        [Then("I should see the following validation messages:")]
        public void ThenIShouldSeeTheFollowingValidationMessages(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var field = row["field"];
                var expectedMessage = row["message"];

                var actualMessage = _addUserModal.GetFieldValidationMessage(field);

                Assert.That(actualMessage, Is.EqualTo(expectedMessage), $"Validation message for field '{field}' is incorrect.");
            }
        }

        [Then("I should see a validation message for {string} with text {string}")]
        public void ThenIShouldSeeAValidationMessageForWithText(string field, string expectedMessage)
        {
            var actualMessage = _addUserModal.GetFieldValidationMessage(field);

            Assert.That(actualMessage, Is.EqualTo(expectedMessage), $"Validation message for field '{field}' is incorrect.");
        }
    }
}
