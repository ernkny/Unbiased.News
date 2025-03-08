using MediatR;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.ContactUs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.ContactUs
{
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, Contact>
    {
        private readonly IContactRepository _contactRepository;

        public GetCustomerByIdHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<Contact> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _contactRepository.GetCustomerMessagesByIdAsync(request.id);
        }
    }
}
