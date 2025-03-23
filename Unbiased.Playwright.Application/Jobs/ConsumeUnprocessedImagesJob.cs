using MediatR;
using Microsoft.Extensions.Configuration;
using Quartz;
using Unbiased.Playwright.Application.Exceptions.Custom;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;

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

        /// <summary>
        /// Initializes a new instance of the ConsumeUnprocessedImagesJob class.
        /// </summary>
        /// <param name="mediator">The mediator instance for handling commands and queries.</param>
        /// <param name="newsService">The service responsible for news-related operations.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="serviceProvider">The service provider instance.</param>
        public ConsumeUnprocessedImagesJob(IMediator mediator, INewsService newsService, IConfiguration configuration, IServiceProvider serviceProvider)
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
            catch (Exception ex) when (ex.Message.Contains("TooManyRequests"))
            {
                throw new TooManyRequestsException("API returned error: TooManyRequests", ex);
            }
            catch (TooManyRequestsException exception)
            {
                Console.WriteLine(exception.Message);
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
