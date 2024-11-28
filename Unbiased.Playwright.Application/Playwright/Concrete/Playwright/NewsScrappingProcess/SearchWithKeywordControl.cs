using Unbiased.Playwright.Application.Playwright.Abstract;
using Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrapping;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrappingProcess
{
    /// <summary>
    /// This class is responsible for handling the search with keyword functionality.
    /// It implements the AbstractHandlerChain interface.
    /// </summary>
    public class SearchWithKeywordControl : AbstractHandlerChain
    {
        private AbstractHandlerChain _abstractHandlerChain;
        private readonly string _keyword;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchWithKeywordControl"/> class.
        /// </summary>
        /// <param name="keyword">The keyword to search with.</param>
        public SearchWithKeywordControl(string keyword)
        {
            _keyword = keyword;
        }

        /// <summary>
        /// Handles the search with keyword functionality.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async override Task<IEnumerable<string>> Handle()
        {
            if (_keyword is not null) 
            {
                var result = await new SearchWithKeywordsMethod().SearchWithKeywords(_keyword);
                return await Task.FromResult(result);
            }
            return await Task.FromResult(Enumerable.Empty<string>());
        }

        /// <summary>
        /// Sets the next handler in the chain.
        /// </summary>
        /// <param name="abstractHandlerChain">The next handler in the chain.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async override Task SetNext(AbstractHandlerChain abstractHandlerChain)
        {
            _abstractHandlerChain = abstractHandlerChain;
            await Task.CompletedTask;
        }
    }
}