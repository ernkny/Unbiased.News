using MediatR;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.News.Application.Services
{
    /// <summary>
    /// Implementation of the INewsService interface for handling news-related operations.
    /// </summary>
    public sealed class NewsService : INewsService
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsService"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance for handling CQRS operations.</param>
        public NewsService(IMediator mediator, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Retrieves all generated news asynchronously.
        /// </summary>
        /// <param name="language">The language code to filter news by.</param>
        /// <returns>A collection of generated news.</returns>
        public async Task<IEnumerable<GeneratedNew>> GetAllGeneratedNewsAsync(string language)
        {
            try
            {

                var result = await _mediator.Send(new GetAllGeneratedNewsQuery(language));
                return result;
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

        /// <summary>
        /// Retrieves all generated news with images asynchronously.
        /// </summary>
        /// <param name="categoryId">The category ID to filter news by.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="language">The language code to filter news by.</param>
        /// <param name="title">Optional title filter for searching news.</param>
        /// <returns>A collection of generated news with images.</returns>
        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync(int categoryId, int pageNumber, string language, string? title)
        {
            try
            {

                var result = await _mediator.Send(new GetAllGeneratedNewsWithImageQuery(categoryId, pageNumber, language, title));
                return result;
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

        /// <summary>
        /// Gets the count of all generated news items with images for a specific category.
        /// </summary>
        /// <param name="categoryId">The category ID to count news items from.</param>
        /// <param name="title">Optional title filter for counting matching news.</param>
        /// <returns>The count of news items matching the criteria.</returns>
        public async Task<int> GetAllGeneratedNewsWithImageCountAsync(int categoryId, string? title)
        {
            try
            {

                var result = await _mediator.Send(new GetAllGeneratedNewsWithImageCountQuery(categoryId, title));
                return result;
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

        /// <summary>
        /// Retrieves the top latest news items from a specific category, excluding the news item with the provided ID.
        /// </summary>
        /// <param name="categoryId">The category ID to retrieve news from.</param>
        /// <param name="uniqUrlPath">The unique URL path of the news item to exclude from results.</param>
        /// <param name="language">The language of the news items to retrieve.</param>
        /// <returns>A collection of the latest top news items from the specified category.</returns>
        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllLastTopGeneratedNewsWithCategoryIdForDetailAsync(int categoryId, string uniqUrlPath, string language)
        {
            try
            {
                var result = await _mediator.Send(new GetAllLastTopGeneratedNewsWithCategoryIdForDetailQuery(categoryId, uniqUrlPath, language));
                return result;
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

        /// <summary>
        /// Retrieves questions and answers related to a specific match.
        /// </summary>
        /// <param name="MatchId">The unique identifier of the match to retrieve Q&A for.</param>
        /// <returns>A collection of questions and answers for the specified match.</returns>
        public async Task<IEnumerable<QuestionAnswerDto>> GetAllQuestionsAndAnswerWithMatchIdAsync(string MatchId)
        {
            try
            {

                var result = await _mediator.Send(new GetAllQuestionsAndAnswerWithMatchIdForGeneratedNewQuery(MatchId));
                return result;
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

        /// <summary>
        /// Retrieves statistical information about all news items in the system.
        /// </summary>
        /// <returns>Statistical data about news items.</returns>
        public async Task<StatisticsDto> GetAllStatisticsAsync()
        {
            try
            {

                var result = await _mediator.Send(new GetNewsStatisticsQuery());
                return result;
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

        /// <summary>
        /// Retrieves banner news items for a specific category and language.
        /// </summary>
        /// <param name="categoryId">The category ID to retrieve banner news from.</param>
        /// <param name="language">The language of the news items to retrieve.</param>
        /// <returns>A collection of banner news items with images.</returns>
        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetBannerGeneratedNewsWithImageAsync(int categoryId, string language)
        {
            try
            {

                var result = await _mediator.Send(new GetBannerGeneratedNewsWithImageQuery(categoryId, language));
                return result;
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

        /// <summary>
        /// Retrieves a specific news item by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the news item.</param>
        /// <returns>The requested news item with its associated image.</returns>
        public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdAsync(string id)
        {
            try
            {

                var result = await _mediator.Send(new GetGeneratedNewsByIdWithImagePathQuery(id));
                return result;
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

        /// <summary>
        /// Retrieves a specific news item by its unique URL.
        /// </summary>
        /// <param name="UniqUrl">The unique URL of the news item.</param>
        /// <returns>The requested news item with its associated image.</returns>
        public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByUniqUrlAsync(string UniqUrl)
        {
            try
            {

                var result = await _mediator.Send(new GetGeneratedNewsByUniqUrlWithImageQuery(UniqUrl));
                return result;
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


        /// <summary>
        /// Retrieves banner news items for a specific category and language.
        /// </summary>
        /// <param name="categoryId">The category ID to retrieve banner news from.</param>
        /// <param name="language">The language of the news items to retrieve.</param>
        /// <returns>A collection of banner news items with images.</returns>
        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsForSiteMapAsync(string language)
        {
            try
            {
                var result = await _mediator.Send(new GetAllGeneratedNewsForSiteMapQuery(language));
                return result;
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
