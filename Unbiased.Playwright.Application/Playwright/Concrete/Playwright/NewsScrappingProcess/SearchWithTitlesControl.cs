using Unbiased.Playwright.Application.Dto.PlaywrightDto;
using Unbiased.Playwright.Application.Playwright.Abstract;
using Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrapping;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrappingProcess
{
    /// <summary>
    /// This class is responsible for handling the search with titles control.
    /// It implements the AbstractHandlerChain interface.
    /// </summary>
    public class SearchWithTitlesControl : AbstractHandlerChain
    {
        private  AbstractHandlerChain _abstractHandlerChain;
        private readonly IEnumerable<string> titlesOfNews;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchWithTitlesControl"/> class.
        /// </summary>
        /// <param name="titlesOfNews">The list of titles of news to search for.</param>
        public SearchWithTitlesControl(IEnumerable<string> titlesOfNews)
        {
            this.titlesOfNews = titlesOfNews;
        }

        /// <summary>
        /// Handles the search with titles.
        /// </summary>
        /// <returns>A list of SaveSearchUrlAndGuidDto objects.</returns>
        public override async Task<List<SaveSearchUrlAndGuidDto>> Handle()
        {
            
            if (titlesOfNews.Any())
            {
                var result = await new SearchWithTitlesMethod().SearchWithTitles(titlesOfNews.ToList());
                return result;
            }

            return await Task.FromResult(Enumerable.Empty<SaveSearchUrlAndGuidDto>().ToList());

        }

        /// <summary>
        /// Sets the next handler in the chain.
        /// </summary>
        /// <param name="abstractHandlerChain">The next handler in the chain.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public override async  Task SetNext(AbstractHandlerChain abstractHandlerChain)
        {
            _abstractHandlerChain = abstractHandlerChain;
            await Task.CompletedTask;
        }
    }
}
