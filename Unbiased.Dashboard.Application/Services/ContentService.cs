using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Dto_s.Content;
using Unbiased.Dashboard.Domain.Model.Aws;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Content;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Application.Services
{
    /// <summary>
    /// Provides methods for managing and retrieving content categories and contents.
    /// </summary>
    public class ContentService : IContentService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly AwsCredentials _awsCredentials;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        ///  Initializes a new instance of the <see cref="ContentService"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="configuration"></param>
        /// <param name="awsOptions"></param>
        /// <param name="serviceProvider"></param>
        public ContentService(IMediator mediator, IConfiguration configuration, IOptions<AwsCredentials> awsOptions, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _configuration = configuration;
            _awsCredentials = awsOptions.Value;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// / Retrieves all content categories from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ContentCategories>> GetAllContentCategoriesAsync()
        {
            try
            {
                return await _mediator.Send(new GetAllContentCategoriesQuery());
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

        /// <summary>
        ///  Retrieves all contents based on the specified parameters such as page number, page size, language, category ID, and processing status.
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="Language"></param>
        /// <param name="CategoryId"></param>
        /// <param name="IsProcess"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ContentSubheadingDto>> GetAllContentsAsync(int PageNumber, int PageSize, string Language, int? CategoryId, bool? IsProcess)
        {
            try
            {
                return await _mediator.Send(new GetAllContentSubheadingQuery(PageNumber, PageSize, Language, CategoryId, IsProcess));
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

        /// <summary>
        ///   Retrieves the total count of all content based on the specified language, category ID, and processing status.
        /// </summary>
        /// <param name="Language"></param>
        /// <param name="CategoryId"></param>
        /// <param name="IsProcess"></param>
        /// <returns></returns>
        public async Task<int> GetAllContentsCountAsync(string Language, int? CategoryId, bool? IsProcess)
        {
            try
            {
                return await _mediator.Send(new GetAllContentsCountQuery(Language, CategoryId, IsProcess));
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

        /// <summary>
        ///  Retrieves the generated content by its unique identifier.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<GeneratedContentDto> GetGeneratedContentByIdAsync(int Id)
        {
            try
            {
                return await _mediator.Send(new GetGeneratedContentByIdQuery(Id));
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
