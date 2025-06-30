using MediatR;
using Quartz;
using Unbiased.Playwright.Application.Interfaces.Playwright;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Application.Jobs
{
    /// <summary>
    /// Quartz job that handles scheduled web scraping operations using Playwright.
    /// This job is configured to not allow concurrent executions.
    /// </summary>
    [DisallowConcurrentExecution]
    public class GetNewsWithPlaywrightWithSearchUrlJob : IJob
    {
        private readonly IPlaywrightScrappingService _playwrightScrappingService;
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the GetNewsWithPlaywrightWithSearchUrlJob class.
        /// </summary>
        /// <param name="playwrightScrappingService">The service responsible for web scraping operations.</param>
        /// <param name="mediator">The mediator instance for handling commands and queries.</param>
        public GetNewsWithPlaywrightWithSearchUrlJob(IPlaywrightScrappingService playwrightScrappingService, IMediator mediator, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _playwrightScrappingService = playwrightScrappingService;
            _mediator = mediator;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Executes the job to scrape news from configured search URLs using Playwright.
        /// Currently commented out to temporarily disable the functionality.
        /// </summary>
        /// <param name="context">The context in which the job is being executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var getUrl = await _mediator.Send(new GetAllActiveUrlsForSearchQuery());
                var getSearchUrlsActiveNextRunTime = getUrl.Where(x => x.NextRunTime < DateTime.Now).Take(2).ToList();
                foreach (var url in getSearchUrlsActiveNextRunTime)
                {
                    var result = await _playwrightScrappingService.PlaywrightScrappingNewsAsync(url);
                    if (result != null)
                    {
                        await _mediator.Send(new AddRangeAllNewsCommand(result));
                    }
                }
                await Task.CompletedTask;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }

        }
    }
}
