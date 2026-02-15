using OpenQA.Selenium;

namespace SeleniumTestFramework.UiTests.Utilities
{
    public static class Retry
    {
        public static void Until(Action action, int retryNumber = 3, int waitInMilliseconds = 500)
        {
            while (retryNumber!=0)
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    if (ex is RetryException || ex is StaleElementReferenceException)
                    {
                        retryNumber--;
                        Thread.Sleep(waitInMilliseconds);

                        continue;
                    }
                    else
                    {
                        throw;
                    }
                }

                break;
            }
        }
    }
}
