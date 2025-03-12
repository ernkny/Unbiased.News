using MediatR;
using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.ContactUs
{
    public record GetAllCustomerMessagesQuery(int PageNumber, int PageSize) : IRequest<IEnumerable<Contact>>;
} 