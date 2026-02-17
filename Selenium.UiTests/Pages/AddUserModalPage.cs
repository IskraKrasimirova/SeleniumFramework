using Common.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.UiTests.Models.UserModels;
using Selenium.UiTests.Utilities.Extensions;

namespace Selenium.UiTests.Pages
{
    public class AddUserModalPage : BasePage
    {
        private IWebElement AddUserHeader => _driver.FindElement(By.XPath("//h5[@id='addUserModalLabel']"));
        private IWebElement TitleDropdown => _driver.FindElement(By.XPath("//select[@id='title']"));
        private IWebElement FirstNameInput => _driver.FindElement(By.XPath("//input[@id='first_name']"));
        private IWebElement SurnameInput => _driver.FindElement(By.XPath("//input[@id='sir_name']"));
        private IWebElement EmailInput => _driver.FindElement(By.XPath("//input[@type='email']"));
        private IWebElement PasswordInput => _driver.FindElement(By.XPath("//input[@type='password']"));
        private IWebElement CountryDropdown => _driver.FindElement(By.XPath("//select[@id='country']"));
        private IWebElement CityInput => _driver.FindElement(By.XPath("//input[@id='city']"));
        private IWebElement IsAdminCheckbox => _driver.FindElement(By.XPath("//input[@type='checkbox' and @id='is_admin']"));
        private IWebElement SubmitButton => _driver.FindElement(By.XPath("//button[@type='submit' and text()='Add User']"));
        private IWebElement FormAlertElement => _driver.FindElement(By.XPath("//div[@id='formAlert' and contains(@class,'alert-danger')]"));

        public AddUserModalPage(IWebDriver driver) : base(driver)
        {
        }

        public void AddUser(AddUserModel model)
        {
            var titleSelect = new SelectElement(TitleDropdown);
            titleSelect.SelectByValue(model.Title);

            FirstNameInput.EnterText(model.FirstName);
            SurnameInput.EnterText(model.Surname);

            var countrySelect = new SelectElement(CountryDropdown);
            countrySelect.SelectByValue(model.Country);

            CityInput.EnterText(model.City);
            EmailInput.EnterText(model.Email);
            PasswordInput.EnterText(model.Password);

            if (model.IsAdmin && !IsAdminCheckbox.Selected)
            {
                IsAdminCheckbox.Click();
            }

            _driver.ScrollToElementAndClick(SubmitButton);
        }

        public string GetFormErrorMessage() => FormAlertElement.Text.Trim();

        public string GetFieldValidationMessage(string field)
        {
            IWebElement element;

            switch (field)
            {
                case "title":
                    element = TitleDropdown;
                    break;
                case "firstName":
                    element = FirstNameInput;
                    break;
                case "surname":
                    element = SurnameInput;
                    break;
                case "country":
                    element = CountryDropdown;
                    break;
                case "city":
                    element = CityInput;
                    break;
                case "email":
                    element = EmailInput;
                    break;
                case "password":
                    element = PasswordInput;
                    break;
                default:
                    throw new ArgumentException($"Unknown field: {field}");
            }

            var messageElement = element.FindElement(By.XPath("./following-sibling::div[@class='invalid-feedback']"));

            return messageElement.Text.Trim();
        }

        public void VerifyModalIsClosed()
        {
            Retry.Until(() =>
            {
                var modal = _driver.FindElement(By.XPath("//div[@id='addUserModal' and contains(@class,'show')]"));
                if (modal != null && modal.Displayed)
                    throw new RetryException("Add User modal is still visible.");
            });
        }

        public void VerifyIsAtAddUserModal()
        {
            Retry.Until(() =>
            {
                if (!AddUserHeader.Displayed)
                    throw new RetryException("Add User modal is not displayed yet.");
            });

            Assert.That(AddUserHeader.Text.Trim(), Is.EqualTo("Add New User"));
        }

        public void VerifyFormAlertMessage(string expectedMessage)
        {
            var actualMessage = GetFormErrorMessage();
            Assert.That(actualMessage, Is.EqualTo(expectedMessage), "The alert message is incorrect.");
        }
    }
}
