namespace Common.Utilities
{
    public static class Retry
    {
        public static void Until(Action action, IList<Exception>? exceptionsToCatch = null, int retryNumber = 3, int waitInMilliseconds = 500)
        {
            while (retryNumber != 0)
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception e)
                {
                    bool isExceptionToCatch = exceptionsToCatch == null ? false :
                        exceptionsToCatch.Any(exception => exception.GetType() == e.GetType());
                    if (e is RetryException || isExceptionToCatch)
                    {
                        retryNumber--;
                        Thread.Sleep(waitInMilliseconds);

                        continue;
                    }
                    else
                        throw;
                }

                break;
            }
        }
    }
}