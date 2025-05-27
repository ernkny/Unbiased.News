using MediatR;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.ContactUs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.ContactUs
{
    /// <summary>
    /// Handler for processing GetCustomerByIdQuery requests to retrieve a specific customer contact.
    /// </summary>
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Contact>
    {
        private readonly IContactRepository _contactRepository;

        /// <summary>
        /// Initializes a new instance of the GetCustomerByIdQueryHandler class.
        /// </summary>
        /// <param name="contactRepository">The contact repository for data access operations.</param>
        public GetCustomerByIdQueryHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        /// <summary>
        /// Handles the GetCustomerByIdQuery request and returns the specified customer contact.
        /// </summary>
        /// <param name="request">The query request containing the customer ID to retrieve.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing the customer contact entity.</returns>
        public async Task<Contact> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _contactRepository.GetCustomerMessagesByIdAsync(request.Id);
        }
    }
} 