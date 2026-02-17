using FluentAssertions;
using Reqnroll;
using RestSharp.ApiTests.Models.Dtos;
using RestSharp.ApiTests.Utilities;

namespace RestSharp.ApiTests.Steps
{
    [Binding]
    public class CommonApiSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public CommonApiSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then("the response status code should be {int}")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            var statusCode = _scenarioContext.Get<int>(ContextConstants.StatusCode); 

            statusCode.Should().Be(expectedStatusCode);
        }

        [Then("the response should contain the following error message {string}")]
        public void ThenTheResponseShouldContainTheFollowingErrorMessage(string errorMessage)
        {
            var response = _scenarioContext.Get<string>(ContextConstants.RawResponse);

            response.Should().Contain(errorMessage, $"Expected the response to contain error message '{errorMessage}'");
        }

        [Then("the response should contain the following message {string}")]
        public void ThenTheResponseShouldContainTheFollowingMessage(string expectedMessage)
        {
            var response = _scenarioContext.Get<string>(ContextConstants.RawResponse);

            response.Should().Contain(expectedMessage, $"Expected the response to contain message '{expectedMessage}'");
        }

        [Then("response should contain error messages:")]
        public void ThenResponseShouldContainErrorMessages(DataTable dataTable)
        {
            var expectedErrorMessages = dataTable.Rows.Select(row => row["ErrorMessage"]);
            var actualErrorsResponse = _scenarioContext.Get<MessageDto>(ContextConstants.UsersResponse);

            actualErrorsResponse.Message.Should().Contain(expectedErrorMessages);
        }
    }
}
