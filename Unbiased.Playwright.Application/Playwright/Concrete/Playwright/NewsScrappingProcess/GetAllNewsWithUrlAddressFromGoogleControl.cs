using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Application.Dto.PlaywrightDto;
using Unbiased.Playwright.Application.Playwright.Abstract;
using Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrapping;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrappingProcess
{
    public class GetAllNewsWithUrlAddressFromGoogleControl : AbstractHandlerChain
    {
        private AbstractHandlerChain _abstractHandlerChain;
        private readonly string _url;

        public GetAllNewsWithUrlAddressFromGoogleControl(string url)
        {
            _url = url;
        }

        public async override Task<List<SaveSearchUrlAndGuidDto>> Handle()
        {
            if (!String.IsNullOrEmpty(_url))
            {
                var result = await new GetAllNewsWithUrlAddressFromGoogleMethod().GetAllNewsWithUrlAddressFromGoogle(_url);
                return result;
            }

            return await Task.FromResult(Enumerable.Empty<SaveSearchUrlAndGuidDto>().ToList());
        }

        public override Task SetNext(AbstractHandlerChain abstractHandlerChain)
        {
            _abstractHandlerChain = abstractHandlerChain;
            return Task.CompletedTask;
        }
    }
}
