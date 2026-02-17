namespace Common.Utilities
{
    public class RetryException: Exception
    {
        public RetryException(string message) : base(message)
        {
        }
    }
}
