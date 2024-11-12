using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Application.Dto.PlaywrightDto;
using Unbiased.Playwright.Application.Playwright.Abstract;
using Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrapping;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrappingProcess
{
    public class SearchWithKeywordControl : AbstractHandlerChain
    {
        private AbstractHandlerChain _abstractHandlerChain;
        private readonly string _keyword;

        public SearchWithKeywordControl(string keyword)
        {
            _keyword = keyword;
        }

        public async override Task<IEnumerable<string>> Handle()
        {
            if (_keyword is not null) 
            {
                var result = await new SearchWithKeywordsMethod().SearchWithKeywords(_keyword);
                return await Task.FromResult(result);
            }
            return await Task.FromResult(Enumerable.Empty<string>());
        }

        public async override Task SetNext(AbstractHandlerChain abstractHandlerChain)
        {
            _abstractHandlerChain = abstractHandlerChain;
            await Task.CompletedTask;
        }
    }
}