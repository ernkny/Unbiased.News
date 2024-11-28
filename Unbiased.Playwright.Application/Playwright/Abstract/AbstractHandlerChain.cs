namespace Unbiased.Playwright.Application.Playwright.Abstract
{
        /// <summary>
    /// Abstract base class for a chain of handlers.
    /// Each handler in the chain is responsible for processing an OrderItem.
    /// </summary>
    public abstract class AbstractHandlerChain
    {
        /// <summary>
        /// Sets the next handler in the chain.
        /// </summary>
        /// <param name="abstractHandlerChain">The next handler in the chain.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public abstract Task SetNext(AbstractHandlerChain abstractHandlerChain);

        /// <summary>
        /// Handles the processing of an OrderItem.
        /// This method should implement the specific handling logic of the derived handler class.
        /// If the item cannot be fully processed, it may be passed to the next handler in the chain.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public abstract Task Handle();
    }
}

