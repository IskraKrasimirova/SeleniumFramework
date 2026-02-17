using Common.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.UiTests.Utilities.Extensions;

namespace Selenium.UiTests.Pages
{
    public class SearchPage : BasePage
    {
        private IWebElement CountryHeader => _driver.FindElement(By.XPath("//h2[contains(text(),'Countries')]"));
        private IWebElement CityHeader => _driver.FindElement(By.XPath("//h2[contains(text(),'Cities')]"));
        private IWebElement SkillsHeader => _driver.FindElement(By.XPath("//h2[contains(text(),'Skills')]"));
        private IWebElement SearchButton => _driver.FindElement(By.XPath("//button[@id='search' and contains(text(),'Search')]"));
        private IWebElement CountryDropdown => _driver.FindElement(By.XPath("//select[@id='availableCountries']"));
        private IWebElement CityDropdown => _driver.FindElement(By.XPath("//select[@id='availableCities']"));

        private IWebElement AddCountryButton => _driver.FindElement(By.XPath("//button[@id='addCountryToSearch']"));
        private IReadOnlyCollection<IWebElement> SearchCountryCheckboxes =>
             _driver.FindElements(By.XPath("//ul[@id='searchedCountries']//input[@type='checkbox']"));

        private IWebElement AddCityButton => _driver.FindElement(By.XPath("//button[@id='addCitiesToSearch']"));
        private IReadOnlyCollection<IWebElement> SearchCityCheckboxes =>
             _driver.FindElements(By.XPath("//ul[@id='searchedCities']//input[@type='checkbox']"));

        private IWebElement? GetCheckboxBySkillName(string skillName) =>
            _driver.FindElements(By.XPath($"//input[@type='checkBox' and @name='skillsToSearch[]' and @value='{skillName}']"))
            .FirstOrDefault();

        private IWebElement? GetNextPageLink() =>
            _driver.FindElements(By.XPath("//ul[@class='pagination']//li[contains(@class,'active')]/following-sibling::li/a[@class='page-link']"))
            .FirstOrDefault();

        public SearchPage(IWebDriver driver) : base(driver)
        {
        }

        public void SelectSkill(string skillName)
        {
            while (true)
            {
                var checkbox = GetCheckboxBySkillName(skillName);

                if (checkbox != null)
                {
                    if (!checkbox.Selected)
                    {
                        _driver.ScrollAndClickJs(checkbox);
                    }
                    return;
                }

                var nextPageLink = GetNextPageLink();

                if (nextPageLink != null)
                {
                    nextPageLink.Click();
                }
                else
                {
                    throw new NoSuchElementException($"Checkbox for skill '{skillName}' not found.");
                }
            }
        }

        public void ClickSearch()
        {
            _driver.ScrollToElementAndClick(SearchButton);
        }

        public void OpenCountryDropdown()
        {
            CountryDropdown.Click();
        }

        public void SelectCountry(string countryName)
        {
            var select = new SelectElement(CountryDropdown);
            select.SelectByText(countryName);

            Retry.Until(() =>
            {
                var options = new SelectElement(CityDropdown).Options;
                if (options.Count == 0)
                    throw new Exception("City dropdown still empty");
            });
        }

        public void SelectCountryForSearch(string countryName)
        {
            SelectCountry(countryName);
            AddCountryButton.Click();

            var checkbox = SearchCountryCheckboxes
                .First(cb => cb.GetAttribute("value") == countryName);

            if (!checkbox.Selected)
                checkbox.Click();
        }

        public void SelectCityForSearch(string cityName)
        {
            var select = new SelectElement(CityDropdown);
            select.SelectByText(cityName);

            AddCityButton.Click();

            var checkbox = SearchCityCheckboxes
                .First(cb => cb.GetAttribute("value") == cityName);

            if (!checkbox.Selected)
                checkbox.Click();
        }

        public void UncheckCountryForSearch(string countryName)
        {
            var checkbox = SearchCountryCheckboxes
                .First(cb => cb.GetAttribute("value") == countryName);

            if (checkbox.Selected)
                checkbox.Click();
        }

        public void UncheckCityForSearch(string cityName)
        {
            var checkbox = SearchCityCheckboxes
                .First(cb => cb.GetAttribute("value") == cityName);

            if (checkbox.Selected)
                checkbox.Click();
        }

        public void VerifyIsAtSearchPage()
        {
            _driver.WaitUntilUrlContains("/search");

            Retry.Until(() =>
            {
                if (!SearchButton.Displayed)
                    throw new RetryException("Search page not loaded yet.");
            });

            Assert.Multiple(() =>
            {
                Assert.That(CountryHeader.Displayed, "Country header is not visible.");
                Assert.That(CityHeader.Displayed, "City header is not visible.");
                Assert.That(SkillsHeader.Displayed, "Skills header is not visible.");
            });
        }

        public void VerifyCountryExists(string countryName)
        {
            var countrySelect = new SelectElement(CountryDropdown);
            var countries = countrySelect.Options.Select(o => o.Text.Trim()).ToList();

            Assert.That(countries, Does.Contain(countryName));
        }

        public void VerifyCityExists(string cityName)
        {
            var citySelect = new SelectElement(CityDropdown);
            var cities = citySelect.Options.Select(o => o.Text.Trim()).ToList();

            Assert.That(cities, Does.Contain(cityName));
        }

        public void VerifyCountryIsActiveForSearch(string countryName)
        {
            var selectedCountries = GetActiveCountriesForSearch();

            Assert.That(selectedCountries, Does.Contain(countryName), $"Country '{countryName}' is not active for search.");
        }

        public void VerifyCityIsActiveForSearch(string cityName)
        {
            var selectedCities = GetActiveCitiesForSearch();

            Assert.That(selectedCities, Does.Contain(cityName), $"City '{cityName}' is not active for search.");
        }

        private List<string> GetActiveCountriesForSearch()
        {
            var selectedCountries = SearchCountryCheckboxes
                .Where(c => c.Selected)
                .Select(c => c.GetAttribute("value"))
                .ToList();

            return selectedCountries;
        }

        private List<string> GetActiveCitiesForSearch()
        {
            var selectedCities = SearchCityCheckboxes
                .Where(c => c.Selected)
                .Select(c => c.GetAttribute("value"))
                .ToList();

            return selectedCities;
        }
    }
}