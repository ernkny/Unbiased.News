using MediatR;
using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.ContactUs
{
    public record GetAllCustomerMessagesQuery(int pageNumber,int pageSize):IRequest<IEnumerable<Contact>>;
}
