using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Selenium.UiTests.Utilities.Extensions
{
    public static class WebDriverExtensions
    {
        public static void EnterText(this IWebElement element, string text)
        {
            element.Clear();
            element.SendKeys(text);
        }

        public static void WaitUntilUrlContains(this IWebDriver driver, string expectedUrlPart, int timeoutInSeconds = 5)
        {
            driver.WaitForPredicate(timeoutInSeconds)
                .Until(ExpectedConditions.UrlContains(expectedUrlPart));
        }

        public static void WaitUntilElementIsVisible(this IWebDriver driver, IWebElement element, int timeoutInSeconds = 5)
        {
            driver.WaitForPredicate(timeoutInSeconds)
                .Until(d => element.Displayed);
        }

        public static void WaitUntilElementIsClickable(this IWebDriver driver, IWebElement element, int timeoutInSeconds = 5)
        {
            driver.WaitForPredicate(timeoutInSeconds)
                .Until(ExpectedConditions.ElementToBeClickable(element));
        }

        public static void WaitUntilValueIsPresent(this IWebDriver driver, IWebElement element, string value, int timeoutInSeconds = 5)
        {
            driver.WaitForPredicate(timeoutInSeconds)
                .Until(ExpectedConditions.TextToBePresentInElementValue(element, value));
        }

        public static void WaitUntilTextIsPresent(this IWebDriver driver, IWebElement element, string expectedText, int timeoutInSeconds = 5) 
        { 
            driver.WaitForPredicate(timeoutInSeconds)
                .Until(ExpectedConditions.TextToBePresentInElement(element, expectedText)); 
        }

        public static void ScrollToElement(this IWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView()", element);
        }

        public static void ScrollToElementAndClick(this IWebDriver driver, IWebElement element)
        {
            driver.ScrollToElement(element);
            element.Click();
        }

        public static void ScrollAndClickJs(this IWebDriver driver, IWebElement element)
        {
            var js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            js.ExecuteScript("arguments[0].click();", element);
        }

        public static void ScrollToElementAndSendText(this IWebDriver driver, IWebElement element, string text)
        {
            driver.ScrollToElement(element);

            element.Clear();
            element.SendKeys(text);
        }

        private static WebDriverWait WaitForPredicate(this IWebDriver driver, int timeoutInSeconds = 10)
        {
            var customWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            customWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            return customWait;
        }
    }
}
