namespace Unbiased.Playwright.Application.Playwright.Abstract
{
    public abstract class AbstractHandlerChain
    {
        public abstract Task SetNext(AbstractHandlerChain abstractHandlerChain);

        /// <summary>
        /// Handles the processing of an OrderItem.
        /// This method should implement the specific handling logic of the derived handler class. If the item cannot be fully processed, it may be passed to the next handler in the chain.
        public abstract Task Handle();
    }
}

