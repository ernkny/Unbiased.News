using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.ContactUs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.ContactUs
{
    public class DeleteCustomerMessageCommandHandler : IRequestHandler<DeleteCustomerMessageCommand, bool>
    {
        private readonly IContactRepository _contactRepository;

        public DeleteCustomerMessageCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<bool> Handle(DeleteCustomerMessageCommand request, CancellationToken cancellationToken)
        {
            return await _contactRepository.DeleteCustomerMessagesByIdAsync(request.Id);
        }
    }
} 