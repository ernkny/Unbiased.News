using MediatR;
using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.ContactUs
{
    /// <summary>
    /// Query record for retrieving all customer messages with pagination support.
    /// </summary>
    /// <param name="PageNumber">The page number for pagination.</param>
    /// <param name="PageSize">The number of items per page for pagination.</param>
    public record GetAllCustomerMessagesQuery(int PageNumber, int PageSize) : IRequest<IEnumerable<Contact>>;
} 