using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Application.Dto.PlaywrightDto;
using Unbiased.Playwright.Application.Playwright.Abstract;
using Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrapping;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrappingProcess
{
    public class GetNewsWithGuidControl : AbstractHandlerChain
    {
        private AbstractHandlerChain _abstractHandlerChain;
        private readonly List<SaveSearchUrlAndGuidDto> _saveSearchUrlAndGuids;

        public GetNewsWithGuidControl(List<SaveSearchUrlAndGuidDto> saveSearchUrlAndGuids)
        {
            _saveSearchUrlAndGuids = saveSearchUrlAndGuids;
        }
        public async override Task<List<News>> Handle()
        {
            var getNewsContent = new GetNewsWithGuidMethod();
            var result=await getNewsContent.GetNewsWithGuid(_saveSearchUrlAndGuids);
            return result;
        }

        public override Task SetNext(AbstractHandlerChain abstractHandlerChain)
        {
            _abstractHandlerChain = abstractHandlerChain;
            return Task.CompletedTask;
        }
    }
}
