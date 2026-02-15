using OpenQA.Selenium;
using SeleniumTestFramework.UiTests.Utilities.Extensions;

namespace SeleniumTestFramework.UiTests.Pages
{
    public class DashboardPage: BasePage
    {
        private IWebElement LoggedUserAnchor => _driver.FindElement(By.XPath("//a[@id='navbarDropdown']"));
        private IWebElement UsernameHeader => _driver.FindElement(By.XPath("//div[contains(@class, 'container-fluid')]/h1"));
        private IWebElement HomeLink => _driver.FindElement(By.XPath("//a[contains(text(), 'Home')]"));
        private IWebElement UsersLink => _driver.FindElement(By.XPath("//a[contains(text(), 'Users')]"));
        private IWebElement SearchLink => _driver.FindElement(By.XPath("//a[contains(text(), 'Search')]"));
        private IWebElement LogoutLink => _driver.FindElement(By.XPath("//a[@class='dropdown-item' and contains(., 'Logout')]"));

        public DashboardPage(IWebDriver driver): base(driver)
        {
        }

        public string GetLoggedUserEmail() => LoggedUserAnchor.Text.Trim();

        public bool IsHomeLinkDisplayed() => HomeLink.Displayed;

        public bool IsUsersLinkDisplayed() => UsersLink.Displayed;

        public bool IsSearchLinkDisplayed() => SearchLink.Displayed;

        public bool IsAddUserLinkDisplayed()
        {
            var elements = _driver.FindElements(By.LinkText("Add User"));
            return elements.Count > 0 && elements[0].Displayed;
        }

        public string GetGreetingText() => UsernameHeader.Text.Trim();

        // Logout dropdown does not open on Dashboard page (index.php).
        // aria-expanded remains false after click.
        // Console error: bootstrap is not defined.
        // This issue apears even with manual clicking on the dropdown.
        // As a workaround, logout is performed via Users page (users.php) where dropdown behaves correctly.
        // The problem was in incomplete/broken index.php file.
        // After recovering the file, the workaround is no more needed.
        public void Logout()
        {
            _driver.WaitUntilElementIsClickable(LoggedUserAnchor);
            LoggedUserAnchor.Click();

            _driver.WaitUntilElementIsClickable(LogoutLink);
            LogoutLink.Click();
        }

        public UsersPage GoToUsersPage()
        {
            UsersLink.Click();

            return new UsersPage(_driver);
        }

        public SearchPage GoToSearchPage()
        {
            SearchLink.Click();

            return new SearchPage(_driver);
        }

        // Validations
        public void VerifyLoggedUserEmailIs(string expectedUserEmail)
        {
            string actualUserEmail = LoggedUserAnchor.Text.Trim();

            Assert.That(actualUserEmail, Is.EqualTo(expectedUserEmail));
        }

        public void VerifyUsernameIs(string username)
        {
            string headerText = UsernameHeader.Text.Trim();
            Assert.That(headerText.Contains(username), Is.True);
        }

        public void VerifyIsAtDashboardPage()
        {
            _driver.WaitUntilUrlContains("/index");
            _driver.WaitUntilElementIsVisible(LoggedUserAnchor);

            Assert.Multiple(() =>
            {
                Assert.That(UsernameHeader.Displayed, "User greeting  header is not visible.");
                Assert.That(HomeLink.Displayed, "Home link is not visible.");
                Assert.That(UsersLink.Displayed, "Users link is not visible.");
                Assert.That(SearchLink.Displayed, "Search link is not visible.");
            });
        }

        public void VerifyUserIsLoggedIn(string email, string username, bool isAdmin)
        {
            _driver.WaitUntilElementIsVisible(LoggedUserAnchor);

            var loggedUserEmail = GetLoggedUserEmail();
            Assert.That(loggedUserEmail, Is.EqualTo(email), "User email is not shown.");

            var greetingText = GetGreetingText();
            Assert.That(greetingText, Does.Contain(username), "The greeting text does not contain the username of the logged-in user.");

            Assert.Multiple(() =>
            {
                Assert.That(IsHomeLinkDisplayed(), "Home link is not displayed.");
                Assert.That(IsUsersLinkDisplayed(), "Users link is not displayed.");
                Assert.That(IsSearchLinkDisplayed(), "Search link is not displayed.");
            });

            if (isAdmin)
            {
                Assert.That(IsAddUserLinkDisplayed(), "Add User link should be visible for admin.");
            }
            else
            {
                Assert.That(IsAddUserLinkDisplayed(), Is.False, "Add User link should NOT be visible for common user.");
            }
        }
    }
}
