namespace SeleniumTestFramework.UiTests.Utilities
{
    public class RetryException: Exception
    {
        public RetryException(string message) : base(message)
        {
        }
    }
}
