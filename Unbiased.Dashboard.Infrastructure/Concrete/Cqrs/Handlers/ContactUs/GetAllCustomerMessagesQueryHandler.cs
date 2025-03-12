using MediatR;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.ContactUs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.ContactUs
{
    public class GetAllCustomerMessagesQueryHandler : IRequestHandler<GetAllCustomerMessagesQuery, IEnumerable<Contact>>
    {
        private readonly IContactRepository _contactRepository;

        public GetAllCustomerMessagesQueryHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<IEnumerable<Contact>> Handle(GetAllCustomerMessagesQuery request, CancellationToken cancellationToken)
        {
            return await _contactRepository.GetAllCustomerMessagesAsync(request.PageNumber, request.PageSize);
        }
    }
} 