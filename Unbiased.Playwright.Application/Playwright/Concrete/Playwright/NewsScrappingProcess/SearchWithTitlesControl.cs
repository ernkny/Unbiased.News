using Unbiased.Playwright.Application.Dto.PlaywrightDto;
using Unbiased.Playwright.Application.Playwright.Abstract;
using Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrapping;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrappingProcess
{
    public  class SearchWithTitlesControl : AbstractHandlerChain
    {
        private  AbstractHandlerChain _abstractHandlerChain;
        private readonly IEnumerable<string> titlesOfNews;

        public SearchWithTitlesControl(IEnumerable<string> titlesOfNews)
        {
            this.titlesOfNews = titlesOfNews;
        }

        public override async Task<List<SaveSearchUrlAndGuidDto>> Handle()
        {
            
            if (titlesOfNews.Any())
            {
                var result = await new SearchWithTitlesMethod().SearchWithTitles(titlesOfNews.ToList());
                return result;
            }

            return await Task.FromResult(Enumerable.Empty<SaveSearchUrlAndGuidDto>().ToList());

        }

        public override async  Task SetNext(AbstractHandlerChain abstractHandlerChain)
        {
            _abstractHandlerChain = abstractHandlerChain;
            await Task.CompletedTask;
        }
    }
}
