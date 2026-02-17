using OpenQA.Selenium;
using Reqnroll;
using Selenium.UiTests.DatabaseOperations.Entities;
using Selenium.UiTests.DatabaseOperations.Operations;
using Selenium.UiTests.Pages;
using Selenium.UiTests.Utilities.Constants;

namespace Selenium.UiTests.Steps
{
    [Binding]
    public class SearchSteps
    {
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;
        private readonly UserOperations _userOperations;  
        private readonly SkillOperations _skillOperations;
        private readonly LocationOperations _locationOperations;
        private readonly SearchPage _searchPage; 

        public SearchSteps(IWebDriver driver, ScenarioContext scenarioContext, UserOperations userOperations, SkillOperations skillOperations, LocationOperations locationOperations, SearchPage searchPage)
        {
            _driver = driver;
            _scenarioContext = scenarioContext;
            _userOperations = userOperations;
            _skillOperations = skillOperations;
            _locationOperations = locationOperations;
            _searchPage = searchPage;
        }

        [Given("a user exists in the database with:")]
        public void GivenAUserExistsInTheDatabaseWith(DataTable table)
        {
            var row = table.Rows[0]; 

            var user = new UserEntity 
            { 
                FirstName = row["firstName"], 
                Surname = row["surname"], 
                Email = row["email"], 
                Country = row["country"], 
                City = row["city"], 
                Title = row["title"], 
                Password = row["password"], 
                IsAdmin = bool.Parse(row["isAdmin"]) }; 

            _userOperations.DeleteUserWithEmail(user.Email); 

            var userId = _userOperations.InsertUser(user);
            user.Id = userId;

            _scenarioContext.Add(ContextConstants.InsertedUser, user);
        }

        [Given("the user has the following skills:")]
        public void GivenTheUserHasTheFollowingSkills(DataTable dataTable)
        {
            var user = _scenarioContext.Get<UserEntity>(ContextConstants.InsertedUser);

            foreach (var row in dataTable.Rows)
            {
                var skillName = row["skillName"];
                var skillCompetence = int.Parse(row["competence"]);

                var skillId = _skillOperations.GetSkillIdByName(skillName);

                _skillOperations.AddSkillToUser(user.Id, skillId, skillCompetence);
            }
        }

        [Given("A country exists in the database with name {string}")]
        public void GivenACountryExistsInTheDatabaseWithName(string countryName)
        {
            _locationOperations.DeleteCountry(countryName);
            _locationOperations.InsertCityAndCountry("TEMP_CITY", countryName);
            _scenarioContext.Add(ContextConstants.InsertedCountry, countryName);
        }

        [Given("A city exists in the database with name {string} in country {string}")]
        public void GivenACityExistsInTheDatabaseWithNameInCountry(string cityName, string countryName)
        {
            _locationOperations.DeleteCity(cityName, countryName);
            _locationOperations.InsertCityAndCountry(cityName, countryName);

            _scenarioContext.Add(ContextConstants.InsertedCountry, countryName);
            _scenarioContext.Add(ContextConstants.InsertedCity, cityName);
        }

        [Given("there are users in the system who have skills")]
        public void GivenThereAreUsersInTheSystemWhoHaveSkills()
        {
            var expectedCount = _userOperations.GetAllUserSkillsCount();
            _scenarioContext.Add(ContextConstants.UserSkillsCount, expectedCount);
        }

        [When("I search for users with skill {string}")]
        public void WhenISearchForUsersWithSkill(string skillName)
        {
            _searchPage.VerifyIsAtSearchPage();
            _searchPage.SelectSkill(skillName);
        }

        [When("I refresh the search page")]
        public void WhenIRefreshTheSearchPage()
        {
            _driver.Navigate().Refresh();
        }

        [When("I open the country dropdown")]
        public void WhenIOpenTheCountryDropdown()
        {
            _searchPage.VerifyIsAtSearchPage();
            _searchPage.OpenCountryDropdown();
        }

        [When("I select country {string}")]
        public void WhenISelectCountry(string countryName)
        {
            _searchPage.VerifyCountryExists(countryName);
            _searchPage.SelectCountry(countryName);
        }

        [When("I select country {string} for search")]
        public void WhenISelectCountryForSearch(string countryName)
        {
            _searchPage.VerifyIsAtSearchPage();
            _searchPage.OpenCountryDropdown();
            _searchPage.VerifyCountryExists(countryName);
            _searchPage.SelectCountryForSearch(countryName);
            _searchPage.VerifyCountryIsActiveForSearch(countryName);
        }

        [When("I select city {string} for search")]
        public void WhenISelectCityForSearch(string cityName)
        {
            _searchPage.VerifyCityExists(cityName);
            _searchPage.SelectCityForSearch(cityName);
            _searchPage.VerifyCityIsActiveForSearch(cityName);
        }

        [When("I perform the search")]
        public void WhenIPerformTheSearch()
        {
            _searchPage.ClickSearch();
        }

        [When("I uncheck country {string} from search list")]
        public void WhenIUncheckCountryFromSearchList(string countryName)
        {
            _searchPage.UncheckCountryForSearch(countryName);
        }

        [When("I uncheck city {string} from search list")]
        public void WhenIUncheckCityFromSearchList(string cityName)
        {
            _searchPage.UncheckCityForSearch(cityName);
        }

        [Then("I should see {string} in the country dropdown")]
        public void ThenIShouldSeeInTheCountryDropdown(string countryName)
        {
            _searchPage.VerifyCountryExists(countryName);
        }

        [Then("I should see {string} in the city dropdown")]
        public void ThenIShouldSeeInTheCityDropdown(string cityName)
        {
            _searchPage.VerifyCityExists(cityName);
        }
    }
}
