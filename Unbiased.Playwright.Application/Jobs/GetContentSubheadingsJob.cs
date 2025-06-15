using Quartz;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Application.Jobs
{
    /// <summary>
    /// Quartz job responsible for generating new content subheadings.
    /// This job generates subheadings for content categories using AI services
    /// and saves them to the database for later content generation.
    /// </summary>
    [DisallowConcurrentExecution]
    public class GetContentSubheadingsJob : IJob
    {

        private readonly IContentService _contentService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the GetContentSubheadingsJob class.
        /// </summary>
        /// <param name="contentService"></param>
        public GetContentSubheadingsJob(IContentService contentService, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _contentService = contentService;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Executes the job to generate new content subheadings.
        /// </summary>
        /// <param name="context">The context in which the job is being executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _contentService.GenerateSubheadingsAndSaveAsync(context.CancellationToken);
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }
    }
}
