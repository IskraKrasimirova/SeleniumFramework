using OpenQA.Selenium;
using SeleniumTestFramework.UiTests.Utilities.Extensions;

namespace SeleniumTestFramework.UiTests.Pages
{
    public class LoginPage: BasePage
    {
        private readonly By PasswordInputLocator = By.XPath("//input[@type='password']");
        // Elements 
        private IWebElement EmailInput => _driver.FindElement(By.XPath("//input[@type='email']"));
        private IWebElement PasswordInput => _driver.FindElement(PasswordInputLocator);
        private IWebElement SubmitButton => _driver.FindElement(By.XPath("//button[@type='submit' and contains(text(), 'Sign In')]"));
        private IWebElement SignUpLink => _driver.FindElement(By.XPath("//a[contains(text(),'Sign Up Here')]"));

        public LoginPage(IWebDriver driver): base(driver)
        {
        }

        // Actions
        public void LoginWith(string email, string password)
        {
            EmailInput.EnterText(email);
            PasswordInput.EnterText(password);

            // _driver.ScrollToElementAndClick(SubmitButton);
            SubmitButton.Click();
        }

        public RegisterPage GoToRegisterPage()
        {
            SignUpLink.Click();
            return new RegisterPage(_driver);
        }

        public string GetValidationMessage() => _driver.FindElement(By.ClassName("alert")).Text;

        public string? GetEmailBrowserValidationMessage() => EmailInput.GetAttribute("validationMessage");

        public string GetPasswordValidationMessage() => _driver.FindElement(By.ClassName("text-danger")).Text;

        public string? GetPasswordBrowserValidationMessage() => PasswordInput.GetAttribute("validationMessage");

        public bool IsPasswordInputEmpty()
        {
            try
            {
                return string.IsNullOrWhiteSpace(PasswordInput.GetAttribute("value"));
            }
            catch (StaleElementReferenceException)
            {
                return string.IsNullOrWhiteSpace(PasswordInput.GetAttribute("value"));
            }
        }

        // Validations
        public void VerifyPasswordInputIsEmpty()
        {
            //string? text = PasswordInput.GetAttribute("value");
            //Assert.That(text, Is.EqualTo(string.Empty), "Password input should be cleared after failed login attempt.");
            Assert.That(IsPasswordInputEmpty(), Is.True, "Password input should be cleared after failed login attempt.");
        }

        public void VerifyErrorMessageIsDisplayed(string errorMessage)
        {
            var errorDialogText = _driver.FindElement(By.ClassName("alert")).Text;
            Assert.That(errorDialogText, Is.EqualTo(errorMessage));
        }

        public void VerifyIsAtLoginPage()
        {
            _driver.WaitUntilUrlContains("/login");

            Assert.Multiple(() =>
            {
                Assert.That(EmailInput.Displayed, "Email input is not visible.");
                Assert.That(PasswordInput.Displayed, "Password input is not visible.");
                Assert.That(SubmitButton.Displayed, "Sign-in button is not visible.");
                Assert.That(SignUpLink.Displayed, "Sign-up link is not visible.");
            });
        }
    }
}
