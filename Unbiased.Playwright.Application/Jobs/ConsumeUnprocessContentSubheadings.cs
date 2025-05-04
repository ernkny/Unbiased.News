using MediatR;
using Microsoft.Extensions.Configuration;
using Quartz;
using Unbiased.Playwright.Application.Exceptions.Custom;
using Unbiased.Playwright.Application.Interfaces;

namespace Unbiased.Playwright.Application.Jobs
{
    /// <summary>
    /// Quartz job responsible for processing unprocessed content subheadings.
    /// This job identifies subheadings that haven't been processed yet and 
    /// generates content for them using the content service.
    /// </summary>
    public class ConsumeUnprocessContentSubheadings : IJob
    {
        private readonly IMediator _mediator;
        private readonly IContentService _contentService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the ConsumeUnprocessContentSubheadings class.
        /// </summary>
        /// <param name="mediator">The mediator instance for sending requests.</param>
        /// <param name="contentService">The content service for generating content.</param>
        /// <param name="configuration">The application configuration.</param>
        public ConsumeUnprocessContentSubheadings(IMediator mediator, IContentService contentService, IConfiguration configuration)
        {
            _mediator = mediator;
            _contentService = contentService;
            _configuration = configuration;
        }

        /// <summary>
        /// Executes the job to process unprocessed content subheadings.
        /// Handles rate limiting by implementing retry logic for too many requests errors.
        /// </summary>
        /// <param name="context">The context in which the job is being executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _contentService.GenerateContentAsync(context.CancellationToken);
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

            }
        }
    }
}
