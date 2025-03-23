using MediatR;
using Microsoft.Extensions.Configuration;
using Quartz;
using System.Runtime.CompilerServices;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.Concrete.ExternalServices;

namespace Unbiased.Playwright.Application.Jobs
{
    /// <summary>
    /// Quartz job responsible for generating and storing daily content data.
    /// This job is configured to not allow concurrent executions.
    /// </summary>
    [DisallowConcurrentExecution]
    public class GetDailyContentDataJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the GetDailyContentDataJob class.
        /// </summary>
        /// <param name="mediator">The mediator instance for handling commands and queries.</param>
        /// <param name="configuration">The application configuration.</param>
        public GetDailyContentDataJob(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        /// <summary>
        /// Executes the job to generate daily content using an external API.
        /// Processes the generated content and stores it in the database with category ID 1.
        /// </summary>
        /// <param name="context">The context in which the job is being executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {

                if (context != null)
                {
                    var dailyContentDataFromGpt = new GptApiExternalService(new HttpClient(), _configuration, _mediator);
                    var content = await dailyContentDataFromGpt.SendDailyInformationToGptAndGetResponse(context.CancellationToken);
                    var contentDetail = new Contents()
                    {
                        ContentCategoryId = 1,
                        ContentDetail = content,
                        CreatedDate = DateTime.Now,
                    };
                    await _mediator.Send(new InsertDailyContentCommand(contentDetail), context.CancellationToken);
                }
                await Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
