using MediatR;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.News.Application.Services
{
    /// <summary>
    /// Implementation of the IContentService interface for managing content operations.
    /// Handles retrieving and processing various types of content including subheadings, horoscopes, and daily content.
    /// </summary>
    public sealed class ContentService : IContentService
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the ContentService class.
        /// </summary>
        /// <param name="mediator">The mediator instance for sending CQRS queries</param>
        public ContentService(IMediator mediator, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Retrieves a paginated collection of content subheadings for a specific category.
        /// </summary>
        /// <param name="categoryId">The ID of the category to retrieve subheadings for</param>
        /// <param name="pageNumber">The page number for pagination</param>
        /// <returns>A collection of content subheadings for the specified category</returns>
        public async Task<IEnumerable<ContentSubHeading>> ContentSubHeadingsAsync(int categoryId, int pageNumber)
        {
            try
            {
                var result = await _mediator.Send(new ContentSubHeadingsQuery(categoryId, pageNumber));
                if (result.Any())
                {

                    return result;
                }
                return Enumerable.Empty<ContentSubHeading>();
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
        /// Gets the total count of subheadings available for a specific category.
        /// </summary>
        /// <param name="categoryId">The ID of the category to count subheadings for</param>
        /// <returns>The total number of subheadings in the category</returns>
        public async Task<int> ContentSubHeadingsCountAsync(int categoryId)
        {
            try
            {
                var result = await _mediator.Send(new ContentSubHeadingsCountQuery(categoryId));
                if (result > 0)
                {

                    return result;
                }
                return 0;
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
        ///  Retrieves all content subheadings with associated images for the home page.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ContentSubHeadingWithImageDto>> GetAllContentWithImageForHomePageAsync(string language)
        {
            try
            {
                var result = await _mediator.Send(new GetAllContentWithImageForHomePageQuery(language));
                if (result.Any())
                {
                    return result;
                }
                return Enumerable.Empty<ContentSubHeadingWithImageDto>();
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
        /// Retrieves the most recent daily horoscope information for all zodiac signs.
        /// </summary>
        /// <returns>A collection of daily horoscope details</returns>
        public async Task<IEnumerable<HoroscopeDailyDetail>> GetDailyLastHoroscopesAsync()
        {
            try
            {

                var getAllHoroscopeDailyInformation = await _mediator.Send(new GetDailyHoroscopeDetailsQuery());
                if (getAllHoroscopeDailyInformation.Any())
                {

                    return getAllHoroscopeDailyInformation;
                }
                return Enumerable.Empty<HoroscopeDailyDetail>();
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
        /// Retrieves generated content details by its unique URL.
        /// </summary>
        /// <param name="uniqUrl">The unique URL identifier for the content</param>
        /// <returns>The content details associated with the URL</returns>
        public async Task<GeneratedContentDto> GetGeneratedContentByUrlAsync(string uniqUrl)
        {
            try
            {
                var result = await _mediator.Send(new GetContentDetailQuery(uniqUrl));
                if (result is not null)
                {
                    return result;
                }
                return null;
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
        /// Retrieves the most recent content entry from the database.
        /// </summary>
        /// <returns>The latest content object</returns>
        /// <exception cref="Exception">Thrown when no content is found</exception>
        public async Task<Contents> GetLastContentAsync()
        {
            try
            {

                var getDailyInformationContentQuery = await _mediator.Send(new GetDailyInformationContentQuery());
                if (getDailyInformationContentQuery is not null)
                {

                    return getDailyInformationContentQuery;
                }
                throw new Exception("No content found");
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
