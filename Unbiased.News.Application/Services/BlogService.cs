using MediatR;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Blog;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.News.Application.Services
{
    /// <summary>
    /// Service for managing blog-related operations.
    /// </summary>
    public sealed class BlogService:IBlogService
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogService"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="serviceProvider"></param>
        public BlogService(IMediator mediator, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Retrieves all blogs with images asynchronously.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="pageNumber"></param>
        /// <param name="searchData"></param>
        /// <returns></returns>
        public async  Task<IEnumerable<BlogWithImageDto>> GetAllBlogsWithImageAsync(string language, int pageNumber, string searchData)
        {
            try
            {
                return await _mediator.Send(new GetAllBlogsWithImageQuery(language, pageNumber, searchData));
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
        ///  Retrieves the count of all blogs with images asynchronously.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="searchData"></param>
        /// <returns></returns>
        public async Task<int> GetAllBlogsWithImageCountAsync(string language, string? searchData)
        {
            try
            {
                return await _mediator.Send(new GetAllBlogsWithImageCountQuery(language, searchData));
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
        ///  Retrieves a blog with image by its unique URL asynchronously.
        /// </summary>
        /// <param name="UniqUrl"></param>
        /// <returns></returns>
        public async Task<BlogWithImageDto> GetBlogWithImageByUniqUrlAsync(string UniqUrl)
        {
            try
            {
                return await _mediator.Send(new GetBlogWithImageByUniqUrlQuery(UniqUrl));
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
