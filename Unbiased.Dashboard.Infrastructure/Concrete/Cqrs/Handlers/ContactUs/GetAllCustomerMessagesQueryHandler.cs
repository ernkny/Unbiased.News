using MediatR;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.ContactUs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.ContactUs
{
    /// <summary>
    /// Handler for processing GetAllCustomerMessagesQuery requests to retrieve all customer messages with pagination.
    /// </summary>
    public class GetAllCustomerMessagesQueryHandler : IRequestHandler<GetAllCustomerMessagesQuery, IEnumerable<Contact>>
    {
        private readonly IContactRepository _contactRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllCustomerMessagesQueryHandler class.
        /// </summary>
        /// <param name="contactRepository">The contact repository for data access operations.</param>
        public GetAllCustomerMessagesQueryHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        /// <summary>
        /// Handles the GetAllCustomerMessagesQuery request and returns a paginated collection of customer messages.
        /// </summary>
        /// <param name="request">The query request containing pagination parameters.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of customer contact entities.</returns>
        public async Task<IEnumerable<Contact>> Handle(GetAllCustomerMessagesQuery request, CancellationToken cancellationToken)
        {
            return await _contactRepository.GetAllCustomerMessagesAsync(request.PageNumber, request.PageSize);
        }
    }
} 