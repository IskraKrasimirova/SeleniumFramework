using FluentAssertions;
using Reqnroll;
using RestSharp;
using SeleniumTestFramework.ApiTests.Apis;
using SeleniumTestFramework.ApiTests.Models.Dtos;
using SeleniumTestFramework.ApiTests.Utils;
using SeleniumTestFramework.ApiTests.Utils.Types;

namespace SeleniumTestFramework.ApiTests.Steps
{
    [Binding]
    public class LoginApiSteps
    {
        private readonly LoginApi _loginApi;
        private readonly ScenarioContext _scenarioContext;

        public LoginApiSteps(ScenarioContext scenarioContext, LoginApi loginApi)
        {
            _scenarioContext = scenarioContext;
            _loginApi = loginApi;
        }

        [Given("I make a post request to login endpoint with valid user credentials")]
        public void GivenIMakeAPostRequestToLoginEndpointWithValidUserCredentials(DataTable dataTable)
        {
            var existingUserCredentials = dataTable.CreateInstance<LoginDto>();
            existingUserCredentials.Password = StringUtils.Sha256(existingUserCredentials.Password);

            var loginResponse = _loginApi.LoginWith(existingUserCredentials);

            _scenarioContext[ContextConstants.StatusCode] = (int)loginResponse.StatusCode;
            _scenarioContext[ContextConstants.LoginResponse] = loginResponse.Data;
        }

        [Given("I make a post request to login endpoint with not valid user credentials {string} and {string}")]
        public void GivenIMakeAPostRequestToLoginEndpointWithNotValidUserCredentialsAnd(string email, string password)
        {
            var loginCredentials = new LoginDto
            {
                Email = email,
                Password = StringUtils.Sha256(password)
            };

            var loginResponse = _loginApi.LoginWith(loginCredentials);

            SaveErrorResponse(loginResponse);
        }

        [Given("I make a get request to login endpoint with valid user credentials")]
        public void GivenIMakeAGetRequestToLoginEndpointWithValidUserCredentials(DataTable dataTable)
        {
            var existingUserCredentials = dataTable.CreateInstance<LoginDto>();
            existingUserCredentials.Password = StringUtils.Sha256(existingUserCredentials.Password);

            var loginResponse = _loginApi.LoginWithGet(existingUserCredentials);

            SaveErrorResponse(loginResponse);
        }

        [Given("I make a post request to login endpoint with body:")]
        public void GivenIMakeAPostRequestToLoginEndpointWithBody(string rawJson)
        {
            var loginResponse = _loginApi.LoginWithRawJson(rawJson);

            SaveErrorResponse(loginResponse);
        }

        [Given("I make a post request to login endpoint with empty body")]
        public void GivenIMakeAPostRequestToLoginEndpointWithEmptyBody()
        {
            var loginResponse = _loginApi.LoginWithEmptyBody();

            SaveErrorResponse(loginResponse);
        }

        [Given("I make a post request to login endpoint with malformed JSON body")]
        public void GivenIMakeAPostRequestToLoginEndpointWithMalformedJSONBody()
        {
            var malformedJsonBody = @"{ ""email"": ""admin@automation.com"", ""password"": ""pass123""";
            var loginResponse = _loginApi.LoginWithRawJson(malformedJsonBody);

            SaveErrorResponse(loginResponse);
        }

        [Given("I make a post request to login endpoint with SQL injection attempt")]
        public void GivenIMakeAPostRequestToLoginEndpointWithSQLInjectionAttempt()
        {
            var loginCredentials = new LoginDto
            {
                Email = "' OR 1=1 --",
                Password = StringUtils.Sha256("anything")
            };

            var loginResponse = _loginApi.LoginWith(loginCredentials);

            SaveErrorResponse(loginResponse);
        }

        [Given("I make a post request to login endpoint with XSS injection payload")]
        public void GivenIMakeAPostRequestToLoginEndpointWithXSSInjectionPayload()
        {
            var loginCredentials = new LoginDto 
            { 
                Email = "<script>alert(1)</script>",
                Password = StringUtils.Sha256("anything")
            }; 

            var loginResponse = _loginApi.LoginWith(loginCredentials);

            SaveErrorResponse(loginResponse);
        }

        [Given("I make a post request to login endpoint with extremely long credentials")]
        public void GivenIMakeAPostRequestToLoginEndpointWithExtremelyLongCredentials()
        {
            var longString = new string('A', 10000);

            var loginCredentials = new LoginDto
            {
                Email = $"{longString}@test.com",
                Password = StringUtils.Sha256(longString)
            };

            var loginResponse = _loginApi.LoginWith(loginCredentials);

            SaveErrorResponse(loginResponse);
        }

        [When("I login with that user")]
        public void WhenILoginWithThatUser()
        {
            var createdUser = _scenarioContext.Get<UserDto>(ContextConstants.CreatedUserData);

            var loginCredentials = new LoginDto
            {
                Email = createdUser.Email,
                Password = createdUser.Password
            };

            var loginResponse = _loginApi.LoginWith(loginCredentials);

            _scenarioContext[ContextConstants.StatusCode] = (int)loginResponse.StatusCode;
            _scenarioContext[ContextConstants.LoginResponse] = loginResponse.Data;
        }

        [Then("the login response should contain the following data:")]
        public void ThenTheLoginResponseShouldContainTheFollowingData(DataTable dataTable)
        {
            var expectedUser = dataTable.CreateInstance<UserDto>();
            var actualLoggedUser = _scenarioContext.Get<UserDto>(ContextConstants.LoginResponse);

            actualLoggedUser.Should().BeEquivalentTo(expectedUser, options =>
                options.Excluding(u => u.Password));
        }

        [Then("the login response should contain correct user's details")]
        public void ThenTheLoginResponseShouldContainCorrectUsersDetails()
        {
            var expectedUser = _scenarioContext.Get<UserDto>(ContextConstants.CreatedUserData);
            var actualUser = _scenarioContext.Get<UserDto>(ContextConstants.LoginResponse);

            actualUser.Should().BeEquivalentTo(expectedUser, options =>
                options.Excluding(u => u.Password));
        }

        private void SaveErrorResponse(RestResponse response)
        {
            _scenarioContext[ContextConstants.StatusCode] = (int)response.StatusCode;
            _scenarioContext[ContextConstants.RawResponse] = response.Content;
        }
    }
}
