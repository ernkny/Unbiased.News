namespace Unbiased.Playwright.Application.Exceptions.Custom
{
    /// <summary>
    ///  Exception thrown when too many requests are made to a service or API, exceeding the allowed rate limit.
    /// </summary>
    public class TooManyRequestsException : Exception
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="TooManyRequestsException"/> class with a default error message.
        /// </summary>
        public TooManyRequestsException()
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="TooManyRequestsException"/> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public TooManyRequestsException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="TooManyRequestsException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public TooManyRequestsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
