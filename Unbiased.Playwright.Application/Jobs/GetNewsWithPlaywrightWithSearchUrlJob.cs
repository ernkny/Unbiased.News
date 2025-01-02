using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Application.Interfaces.Playwright;

namespace Unbiased.Playwright.Application.Jobs
{
    public class GetNewsWithPlaywrightWithSearchUrlJob : IJob
    {
        private readonly IPlaywrightScrappingService _playwrightScrappingService;

        public GetNewsWithPlaywrightWithSearchUrlJob(IPlaywrightScrappingService playwrightScrappingService)
        {
            _playwrightScrappingService = playwrightScrappingService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
           var result= await _playwrightScrappingService.PlaywrightScrappingNewsAndAddRangeNewsAsync();
        }
    }
}
