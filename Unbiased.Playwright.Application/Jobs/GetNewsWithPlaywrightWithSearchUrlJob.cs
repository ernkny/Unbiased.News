using MediatR;
using Quartz;
using Unbiased.Playwright.Application.Interfaces.Playwright;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;

namespace Unbiased.Playwright.Application.Jobs
{
    public class GetNewsWithPlaywrightWithSearchUrlJob : IJob
    {
        private readonly IPlaywrightScrappingService _playwrightScrappingService; 
        private readonly IMediator _mediator;

        public GetNewsWithPlaywrightWithSearchUrlJob(IPlaywrightScrappingService playwrightScrappingService, IMediator mediator)
        {
            _playwrightScrappingService = playwrightScrappingService;
            _mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var getUrl = await _mediator.Send(new GetAllActiveUrlsForSearchQuery());
                var getSearchUrlsActiveNextRunTime = getUrl.Where(x => x.NextRunTime < DateTime.UtcNow).ToList();
                var getNewsNoneProssed= await _mediator.Send(new GetAllNewsByNotIncludedProcessQuery());
                foreach (var url in getSearchUrlsActiveNextRunTime)
                {
                    var checkNewsNoneProssed = getNewsNoneProssed.Count()>0 ? getNewsNoneProssed.Any(x=>x.CategoryId==url.categoryId):false ;
                    if (checkNewsNoneProssed)
                    {
                        var result = await _playwrightScrappingService.PlaywrightScrappingNewsAsync(url);
                        if (result != null)
                        {
                            await _mediator.Send(new AddRangeAllNewsCommand(result));
                        }
                    }
                    
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error in GetNewsWithPlaywrightWithSearchUrlJob");
                throw;
            }
            
        }
    }
}
