using OpenQA.Selenium;
using Reqnroll;
using Selenium.UiTests.DatabaseOperations.Entities;
using Selenium.UiTests.DatabaseOperations.Operations;
using Selenium.UiTests.Pages;
using Selenium.UiTests.Utilities.Constants;

namespace Selenium.UiTests.Steps
{
    [Binding]
    public class SearchResultSteps
    {
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;
        private readonly SearchPage _searchPage;
        private readonly SearchResultPage _searchResultPage;
        private readonly UserOperations _userOperations;

        public SearchResultSteps(IWebDriver driver, ScenarioContext scenarioContext, SearchPage searchPage, SearchResultPage searchResultPage, UserOperations userOperations)
        {
            _driver = driver;
            _scenarioContext = scenarioContext;
            _searchPage = searchPage;
            _searchResultPage = searchResultPage;
            _userOperations = userOperations;
        }

        [Then("all results should contain skill {string}")]
        public void ThenAllResultsShouldContainSkill(string skillName)
        {
            _searchResultPage.VerifyIsAtSearchResultPage();
            _searchResultPage.VerifyResultsTableIsVisible();
            _searchResultPage.VerifyAllRowsHaveSkill(skillName);
        }

        [Then("I should see the created user in the search results")]
        public void ThenIShouldSeeTheCreatedUserInTheSearchResults()
        {
            var user = _scenarioContext.Get<UserEntity>(ContextConstants.InsertedUser);
            _searchResultPage.VerifyUserExists(user.Email);
        }

        [Then("all results should contain country {string}")]
        public void ThenAllResultsShouldContainCountry(string countryName)
        {
            _searchResultPage.VerifyIsAtSearchResultPage();
            _searchResultPage.VerifyResultsTableIsVisible();
            _searchResultPage.VerifyAllRowsHaveCountry(countryName);
        }

        [Then("all results should contain only countries:")]
        public void ThenAllResultsShouldContainOnlyCountries(DataTable dataTable)
        {
            var expectedCountries = dataTable.Rows.
                Select(r => r["Country"].Trim())
                .ToList();

            _searchResultPage.VerifyRowsContainOnlyCountries(expectedCountries);
        }


        [Then("all results should contain only cities:")]
        public void ThenAllResultsShouldContainOnlyCities(DataTable dataTable)
        {
            var expectedCities = dataTable.Rows.
                Select(r => r["City"].Trim())
                .ToList();

            _searchResultPage.VerifyRowsContainOnlyCities(expectedCities);
        }

        [Then("no users should be found")]
        public void ThenNoUsersShouldBeFound()
        {
            _searchResultPage.VerifyIsAtSearchResultPage();
            _searchResultPage.VerifyNoUsersFound();
        }

        [Then("I should see a message with the following text {string}")]
        public void ThenIShouldSeeAMessageWithTheFollowingText(string expectedMessage)
        {
            _searchResultPage.VerifyInfoMessage(expectedMessage);
        }

        [Then("the results should show every skill for every user")]
        public void ThenTheResultsShouldShowEverySkillForEveryUser()
        {
            var actualTableRowsCount = _searchResultPage.GetCountOfRowsInResultTable();
            var expectedRowsCount = _scenarioContext.Get<int>(ContextConstants.UserSkillsCount);

            Assert.That(actualTableRowsCount, Is.EqualTo(expectedRowsCount));

            var uiRows = _searchResultPage.GetAllRowsOfResultsTable();
            var dbRows = _userOperations.GetAllUserSkillRecords();

            foreach (var uiRow in uiRows)
            {
                bool existsInDb = dbRows.Any(db =>
                db.FirstName == uiRow.FirstName &&
                db.Surname == uiRow.Surname &&
                db.Email == uiRow.Email &&
                db.Country == uiRow.Country &&
                db.City == uiRow.City &&
                db.Skill == uiRow.Skill &&
                db.SkillCategory == uiRow.SkillCategory);

                Assert.That(existsInDb, $"UI row does not exist in DB: {uiRow.Email} - {uiRow.Skill}");
            }
        }

        [Then("the results should not contain this user")]
        public void ThenTheResultsShouldNotContainThisUser()
        {
            var user = _scenarioContext.Get<UserEntity>(ContextConstants.InsertedUser);
            _searchResultPage.VerifyUserDoesNotExist(user.Email);
        }
    }
}
