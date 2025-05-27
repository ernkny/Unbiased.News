using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.ContactUs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.ContactUs
{
    /// <summary>
    /// Handler for processing DeleteCustomerMessageCommand requests to delete customer messages.
    /// </summary>
    public class DeleteCustomerMessageCommandHandler : IRequestHandler<DeleteCustomerMessageCommand, bool>
    {
        private readonly IContactRepository _contactRepository;

        /// <summary>
        /// Initializes a new instance of the DeleteCustomerMessageCommandHandler class.
        /// </summary>
        /// <param name="contactRepository">The contact repository for data access operations.</param>
        public DeleteCustomerMessageCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        /// <summary>
        /// Handles the DeleteCustomerMessageCommand request and deletes the specified customer message.
        /// </summary>
        /// <param name="request">The command request containing the customer message ID to delete.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        public async Task<bool> Handle(DeleteCustomerMessageCommand request, CancellationToken cancellationToken)
        {
            return await _contactRepository.DeleteCustomerMessagesByIdAsync(request.Id);
        }
    }
} 