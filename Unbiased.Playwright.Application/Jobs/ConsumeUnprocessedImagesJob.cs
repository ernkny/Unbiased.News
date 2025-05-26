using MediatR;
using Quartz;
using Unbiased.Playwright.Application.Exceptions.Custom;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Application.Jobs
{
    /// <summary>
    /// Quartz job responsible for processing unprocessed images for generated news content.
    /// This job is configured to not allow concurrent executions.
    /// </summary>
    [DisallowConcurrentExecution]
    public class ConsumeUnprocessedImagesJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly INewsService _newsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the ConsumeUnprocessedImagesJob class.
        /// </summary>
        /// <param name="mediator">The mediator instance for handling commands and queries.</param>
        /// <param name="newsService">The service responsible for news-related operations.</param>
        /// <param name="serviceProvider">The service provider instance.</param>
        public ConsumeUnprocessedImagesJob(IMediator mediator, INewsService newsService,IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _newsService = newsService;
        }

        /// <summary>
        /// Executes the job to process news items that need images generated.
        /// Handles rate limiting by implementing retry logic for too many requests errors.
        /// </summary>
        /// <param name="context">The context in which the job is being executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var images = await _mediator.Send(new GetImagesWithNoneHasGeneratedQuery());
                if (images.Any())
                {
                    foreach (var image in images) {

                        await _newsService.GenerateImagesWhenAllNewsHasGeneratedAsync(context.CancellationToken);
                    }

                }
                await Task.CompletedTask;
            }
            catch (Exception exception) when (exception.Message.Contains("TooManyRequests"))
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
            catch (TooManyRequestsException exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        }
    }
}
