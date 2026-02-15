using FluentAssertions;
using FluentAssertions.Execution;
using Reqnroll;
using SeleniumTestFramework.ApiTests.Apis;
using SeleniumTestFramework.ApiTests.Models.Dtos;
using SeleniumTestFramework.ApiTests.Models.Factories;
using SeleniumTestFramework.ApiTests.Utils;
using StringUtils = SeleniumTestFramework.ApiTests.Utils.Types.StringUtils;


namespace SeleniumTestFramework.ApiTests.Steps
{
    [Binding]
    public class UsersApiSteps
    {
        private readonly UsersApi _usersApi;
        private readonly ScenarioContext _scenarioContext;
        private readonly IUserFactory _userFactory;

        public UsersApiSteps(ScenarioContext scenarioContext, UsersApi usersApi, IUserFactory userFactory)
        {
            _scenarioContext = scenarioContext;
            _usersApi = usersApi;
            _userFactory = userFactory;
        }

        [Given("I make a get request to users endpoint with id {int}")]
        public void GivenIMakeAGetRequestToUsersEndpointWithId(int id)
        {
            var response = _usersApi.GetUserById(id);
            var responseStatusCode = (int)response.StatusCode;

            _scenarioContext.Add(ContextConstants.StatusCode, responseStatusCode);

            if (response.IsSuccessful)
            {
                _scenarioContext.Add(ContextConstants.UsersResponse, response.Data);
            }

            _scenarioContext.Add(ContextConstants.RawResponse, response.Content);
        }

        [Given("I make a post request to users endpoint with the following data:")]
        public void GivenIMakeAPostRequestToUsersEndpointWithTheFollowingData(DataTable dataTable)
        {
            var expectedUser = dataTable.CreateInstance<UserDto>();
            var timespan = DateTime.Now.ToFileTime();
            expectedUser.Email = expectedUser.Email.Replace("@", $"{timespan}@");
            expectedUser.Password = StringUtils.Sha256(expectedUser.Password);

            var createUserResponse = _usersApi.CreateUser<UserDto>(expectedUser);

            var responseStatusCode = (int)createUserResponse.StatusCode;
            _scenarioContext.Add(ContextConstants.StatusCode, responseStatusCode);

            var responseBody = createUserResponse.Data;
            _scenarioContext.Add(ContextConstants.UsersResponse, responseBody);
        }

        [Given("I make a post request to users endpoint with invalid data {string} {string}")]
        public void GivenIMakeAPostRequestToUsersEndpointWithInvalidData(string field, string value)
        {
            var newUser = _userFactory.CreateDefault();

            switch (field)
            {
                case "Title":
                    newUser.Title = value;
                    break;
                case "FirstName":
                    newUser.FirstName = value;
                    break;
                case "SirName":
                    newUser.SirName = value;
                    break;
                case "Country":
                    newUser.Country = value;
                    break;
                case "Email":
                    newUser.Email = value;
                    break;
                default:
                    throw new ArgumentException($"Unknown field: {field}");
            }

            var createUserResponse = _usersApi.CreateUser<UserDto>(newUser);

            _scenarioContext.Add(ContextConstants.StatusCode, (int)createUserResponse.StatusCode);
            _scenarioContext.Add(ContextConstants.RawResponse, createUserResponse.Content);
        }

        [Given("I create a new user via the API")]
        public void GivenICreateANewUserViaTheAPI()
        {
            var newUser = _userFactory.CreateDefault();
            var createUserResponse = _usersApi.CreateUser<UserDto>(newUser);

            using (new AssertionScope())
            {
                createUserResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
                createUserResponse.Data.Should().NotBeNull();
                createUserResponse.Data.Id.Should().BeGreaterThan(0);
            }

            _scenarioContext.Add(ContextConstants.CreatedUserId, createUserResponse.Data.Id);
            // For Update, Create user with existing email
            _scenarioContext.Add(ContextConstants.CreatedUserData, createUserResponse.Data);
        }

        [Given("I make a get request to users endpoint")]
        public void GivenIMakeAGetRequestToUsersEndpoint()
        {
            var response = _usersApi.GetAllUsers();

            _scenarioContext.Add(ContextConstants.StatusCode, (int)response.StatusCode);
            _scenarioContext.Add(ContextConstants.UsersResponse, response.Data);
        }

        [Given("I make a post request to users endpoint with Title {string}")]
        public void GivenIMakeAPostRequestToUsersEndpointWithTitle(string title)
        {
            var newUser = _userFactory.CreateCustom(title: title);
            var createUserResponse = _usersApi.CreateUser<UserDto>(newUser);

            _scenarioContext.Add(ContextConstants.StatusCode, (int)createUserResponse.StatusCode);
            _scenarioContext.Add(ContextConstants.RawResponse, createUserResponse.Content);
        }

        [Given("I make a post request to users endpoint with Country {string} and City {string}")]
        public void GivenIMakeAPostRequestToUsersEndpointWithCountryAndCity(string countryName, string cityName)
        {
            var newUser = _userFactory.CreateCustom(country: countryName, city: cityName);
            var createUserResponse = _usersApi.CreateUser<UserDto>(newUser);

            _scenarioContext.Add(ContextConstants.StatusCode, (int)createUserResponse.StatusCode);
            _scenarioContext.Add(ContextConstants.RawResponse, createUserResponse.Content);
        }

        [Given("I make a post request to users endpoint with empty mandatory fields")]
        public void GivenIMakeAPostRequestToUsersEndpointWithEmptyMandatoryFields()
        {
            var newUser = new UserDto
            {
                Title = "",
                FirstName = "",
                SirName = "",
                Email = "",
                Password = "",
                Country = ""
            };

            var createUserResponse = _usersApi.CreateUser<MessageDto>(newUser);

            _scenarioContext[ContextConstants.StatusCode] = (int)createUserResponse.StatusCode;
            _scenarioContext[ContextConstants.UsersResponse] = createUserResponse.Data;
        }

        [When("I delete that user")]
        public void WhenIDeleteThatUser()
        {
            var id = _scenarioContext.Get<int>(ContextConstants.CreatedUserId);
            var deleteResponse = _usersApi.DeleteUserById(id);

            _scenarioContext[ContextConstants.StatusCode] = (int)deleteResponse.StatusCode;
            _scenarioContext[ContextConstants.RawResponse] = deleteResponse.Content;
        }

        [When("I make a Delete request to users endpoint with id {int}")]
        public void WhenIMakeADeleteRequestToUsersEndpointWithId(int id)
        {
            var deleteResponse = _usersApi.DeleteUserById(id);

            _scenarioContext.Add(ContextConstants.StatusCode, (int)deleteResponse.StatusCode);
            _scenarioContext.Add(ContextConstants.RawResponse, deleteResponse.Content);
        }

        [When("I update that user with valid data")]
        public void WhenIUpdateThatUserWithValidData()
        {
            var id = _scenarioContext.Get<int>(ContextConstants.CreatedUserId);
            var originalUser = _scenarioContext.Get<UserDto>(ContextConstants.CreatedUserData);

            var updatedData = _userFactory.CreateCustom(
                title: "Mrs.",
                firstName: "Francesca",
                surname: originalUser.SirName,
                country: "Italy",
                city: "Rome",
                email: originalUser.Email
                );

            var updateResponse = _usersApi.UpdateUser(id, updatedData);

            _scenarioContext[ContextConstants.StatusCode] = (int)updateResponse.StatusCode;
            _scenarioContext[ContextConstants.UsersResponse] = updateResponse.Data;
            _scenarioContext[ContextConstants.UpdatedUserData] = updatedData;
        }

        [When("I update that user with invalid data {string} {string}")]
        public void WhenIUpdateThatUserWithInvalidData(string field, string value)
        {
            var id = _scenarioContext.Get<int>(ContextConstants.CreatedUserId);
            var originalUser = _scenarioContext.Get<UserDto>(ContextConstants.CreatedUserData);

            var updatedData = _userFactory.CreateCustom(
                title: originalUser.Title,
                firstName: originalUser.FirstName,
                surname: originalUser.SirName,
                country: originalUser.Country,
                city: originalUser.City,
                email: originalUser.Email
                );

            switch (field)
            {
                case "Title":
                    updatedData.Title = value;
                    break;
                case "FirstName":
                    updatedData.FirstName = value;
                    break;
                case "SirName":
                    updatedData.SirName = value;
                    break;
                case "Country":
                    updatedData.Country = value;
                    break;
                case "City":
                    updatedData.City = value;
                    break;
                case "Email":
                    updatedData.Email = value;
                    break;
                default:
                    throw new ArgumentException($"Unknown field: {field}");
            }

            var updateResponse = _usersApi.UpdateUser(id, updatedData);

            _scenarioContext[ContextConstants.StatusCode] = (int)updateResponse.StatusCode;
            _scenarioContext[ContextConstants.RawResponse] = updateResponse.Content;
        }

        [When("I update that user with existing email {string}")]
        public void WhenIUpdateThatUserWithExistingEmail(string email)
        {
            var id = _scenarioContext.Get<int>(ContextConstants.CreatedUserId);
            var originalUser = _scenarioContext.Get<UserDto>(ContextConstants.CreatedUserData);

            var updatedData = _userFactory.CreateCustom(
                title: originalUser.Title,
                firstName: originalUser.FirstName,
                surname: originalUser.SirName,
                country: originalUser.Country,
                city: originalUser.City,
                email: email
            );

            var updateResponse = _usersApi.UpdateUser(id, updatedData);

            _scenarioContext[ContextConstants.StatusCode] = (int)updateResponse.StatusCode;
            _scenarioContext[ContextConstants.RawResponse] = updateResponse.Content;
        }

        [When("I try to create another user with the same email")]
        public void WhenITryToCreateAnotherUserWithTheSameEmail()
        {
            var existingUser = _scenarioContext.Get<UserDto>(ContextConstants.CreatedUserData);

            var newUser = _userFactory.CreateCustom(email: existingUser.Email);
            var createUserResponse = _usersApi.CreateUser<UserDto>(newUser);

            _scenarioContext[ContextConstants.StatusCode] = (int)createUserResponse.StatusCode;
            _scenarioContext[ContextConstants.RawResponse] = createUserResponse.Content;
        }

        [When("I make an Update request to users endpoint with id {int}")]
        public void WhenIMakeAnUpdateRequestToUsersEndpointWithId(int id)
        {
            var updatedData = _userFactory.CreateDefault();

            var updateResponse = _usersApi.UpdateUser(id, updatedData);

            _scenarioContext[ContextConstants.StatusCode] = (int)updateResponse.StatusCode;
            _scenarioContext[ContextConstants.RawResponse] = updateResponse.Content;
        }

        [Then("users response should contain the following data:")]
        public void ThenUsersResponseShouldContainTheFollowingData(DataTable dataTable)
        {
            var expectedUser = dataTable.CreateInstance<UserDto>();
            expectedUser.Password = StringUtils.Sha256(expectedUser.Password);

            var actualUser = _scenarioContext.Get<UserDto>(ContextConstants.UsersResponse);

            actualUser.Id.Should().Be(expectedUser.Id, "User ID does not match the expected user");

            using (new AssertionScope())
            {
                actualUser.FirstName.Should().Be(expectedUser.FirstName);
                actualUser.SirName.Should().Be(expectedUser.SirName);
                actualUser.Title.Should().Be(expectedUser.Title);
                actualUser.Email.Should().Be(expectedUser.Email);
                actualUser.Password.Should().Be(expectedUser.Password);
                actualUser.IsAdmin.Should().Be(expectedUser.IsAdmin);
                actualUser.Country.Should().Be(expectedUser.Country);
                actualUser.City.Should().Be(expectedUser.City);
            }    
        }

        [Then("create users response should contain the following data:")]
        public void ThenCreateUsersResponseShouldContainTheFollowingData(DataTable dataTable)
        {
            var expectedUser = dataTable.CreateInstance<UserDto>();
            var actualUser = _scenarioContext.Get<UserDto>(ContextConstants.UsersResponse);

            actualUser.Should().BeEquivalentTo(
            expectedUser,
            options => options
                .Excluding(u => u.Id)
                .Excluding(u => u.Password)
                .Excluding(u => u.Email)
            );

            using (new AssertionScope())
            {
                actualUser.Email.Should().EndWith("@automation.com");
                actualUser.Password.Should().NotBe("pass123");
                actualUser.Password.Should().HaveLength(64); // SHA256 hex
            }
        }

        [Then("I make a get request to users endpoint with that id")]
        public void ThenIMakeAGetRequestToUsersEndpointWithThatId()
        {
            var id = _scenarioContext.Get<int>(ContextConstants.CreatedUserId);
            var getResponse = _usersApi.GetUserById(id);

            _scenarioContext[ContextConstants.StatusCode] = (int)getResponse.StatusCode;
            _scenarioContext[ContextConstants.RawResponse] = getResponse.Content;

            if (getResponse.IsSuccessful && getResponse.Data is not null)
            {
                _scenarioContext[ContextConstants.UsersResponse] = getResponse.Data;
            }
        }

        [Then("the updated user should have the new data")]
        public void ThenTheUpdatedUserShouldHaveTheNewData()
        {
            var id = _scenarioContext.Get<int>(ContextConstants.CreatedUserId);
            var actualUser = _scenarioContext.Get<UserDto>(ContextConstants.UsersResponse);

            actualUser.Id.Should().Be(id);

            var expectedUser = _scenarioContext.Get<UserDto>(ContextConstants.UpdatedUserData);

            //using (new AssertionScope())
            //{
            //    actualUser.Title.Should().Be(expectedUser.Title);
            //    actualUser.FirstName.Should().Be(expectedUser.FirstName);
            //    actualUser.SirName.Should().Be(expectedUser.SirName);
            //    actualUser.Country.Should().Be(expectedUser.Country);
            //    actualUser.City.Should().Be(expectedUser.City);
            //    actualUser.Email.Should().Be(expectedUser.Email);
            //}

            actualUser.Should().BeEquivalentTo(expectedUser,
                options => options
                .Excluding(u => u.Id)
                .Excluding(u => u.Password)
                );
        }

        [Then("users response should contain non-empty list of users")]
        public void ThenUsersResponseShouldContainNon_EmptyListOfUsers()
        {
            var usersList = _scenarioContext.Get<IReadOnlyCollection<UserDto>>(ContextConstants.UsersResponse);

            usersList.Should().NotBeNullOrEmpty();
        }

        [Then("each user in the list should have valid data")]
        public void ThenEachUserInTheListShouldHaveValidData()
        {
            var users = _scenarioContext.Get<IReadOnlyCollection<UserDto>>(ContextConstants.UsersResponse);

            foreach (var user in users)
            {
                using (new AssertionScope())
                {
                    user.Id.Should().BeGreaterThan(0);
                    user.Title.Should().BeOneOf(["Mr.", "Mrs."]);
                    user.FirstName.Should().NotBeNullOrWhiteSpace();
                    user.SirName.Should().NotBeNullOrWhiteSpace();
                    user.Country.Should().NotBeNullOrWhiteSpace();
                    user.Email.Should().NotBeNullOrWhiteSpace();
                    user.Password.Should().NotBeNullOrWhiteSpace();
                }
            }
        }
    }
}
