using Quartz;
using Unbiased.Playwright.Application.Interfaces;

namespace Unbiased.Playwright.Application.Jobs
{
    /// <summary>
    /// Quartz job responsible for generating new content subheadings.
    /// This job generates subheadings for content categories using AI services
    /// and saves them to the database for later content generation.
    /// </summary>
    public class GetContentSubheadingsJob : IJob
    {

        private readonly IContentService _contentService;

        /// <summary>
        /// Initializes a new instance of the GetContentSubheadingsJob class.
        /// </summary>
        /// <param name="contentService"></param>
        public GetContentSubheadingsJob(IContentService contentService)
        {
            _contentService = contentService;
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
