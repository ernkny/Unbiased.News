using MediatR;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.ContactUs;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.ContactUs;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Application.Services
{
    /// <summary>
    /// Service implementation for contact operations providing functionality for customer message management with error handling and logging.
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the ContactService class.
        /// </summary>
        /// <param name="mediator">The mediator for handling CQRS operations.</param>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        public ContactService(IMediator mediator, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Deletes a customer message by its unique identifier with error handling and logging.
        /// </summary>
        /// <param name="id">The unique identifier of the customer message to delete.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during customer message deletion.</exception>
        public async Task<bool> DeleteCustomerMessagesByIdAsync(int id)
        {
            try
            {
                return await _mediator.Send(new DeleteCustomerMessageCommand(id));
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
        /// Retrieves all customer messages with pagination support, error handling and logging.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page for pagination.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of customer contact entities.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during customer messages retrieval.</exception>
        public async Task<IEnumerable<Contact>> GetAllCustomerMessagesAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _mediator.Send(new GetAllCustomerMessagesQuery(pageNumber, pageSize));
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
        /// Retrieves a specific customer message by its unique identifier with error handling and logging.
        /// </summary>
        /// <param name="id">The unique identifier of the customer message to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation containing the customer contact entity.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during customer message retrieval.</exception>
        public async Task<Contact> GetCustomerMessagesByIdAsync(int id)
        {
            try
            {
                return await _mediator.Send(new GetCustomerByIdQuery(id));
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
