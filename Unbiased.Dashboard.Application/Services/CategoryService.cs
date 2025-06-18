using MediatR;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Categories;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Application.Services
{
    /// <summary>
    /// Service implementation for category operations providing functionality for category management with logging support.
    /// </summary>
    public sealed class CategoryService : ICategoryService
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the CategoryService class.
        /// </summary>
        /// <param name="mediator">The mediator for handling CQRS operations.</param>
        public CategoryService(IMediator mediator, IEventAndActivityLog eventAndActivityLog)
        {
            _mediator = mediator;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Retrieves all categories from the system with error handling and logging.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing a collection of category entities.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during category retrieval.</exception>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _mediator.Send(new GetAllCategoriesQuery());
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
