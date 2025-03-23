using MediatR;
using Microsoft.Extensions.Configuration;
using Quartz;
using Unbiased.Playwright.Application.Exceptions.Custom;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.Concrete.ExternalServices;

namespace Unbiased.Playwright.Application.Jobs
{
    /// <summary>
    /// Quartz job responsible for processing unprocessed news items.
    /// This job is configured to not allow concurrent executions.
    /// </summary>
    [DisallowConcurrentExecution]
    public class ConsumeUnprocessedNewsJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly INewsService _newsService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the ConsumeUnprocessedNewsJob class.
        /// </summary>
        /// <param name="mediator">The mediator instance for handling commands and queries.</param>
        /// <param name="newsService">The service responsible for news-related operations.</param>
        /// <param name="configuration">The application configuration.</param>
        public ConsumeUnprocessedNewsJob(IMediator mediator, INewsService newsService, IConfiguration configuration)
        {
            _mediator = mediator;
            _newsService = newsService;
            _configuration = configuration;
        }

        /// <summary>
        /// Executes the job to process unprocessed news items by generating content using an external API.
        /// Handles rate limiting by implementing retry logic for too many requests errors.
        /// </summary>
        /// <param name="context">The context in which the job is being executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var checkNews = await _mediator.Send(new GetAllNewsByNotIncludedProcessQuery());
                if (checkNews.Any())
                {
                    var combinedNews = await _mediator.Send(new GetAllNewsCombinedDetailsQuery(), context.CancellationToken);
                    var externalServiceSend = new GptApiExternalService(new HttpClient(), _configuration, _mediator);
                    await _newsService.GenerateNewsWithApiAsync(combinedNews, context.CancellationToken, externalServiceSend);
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
