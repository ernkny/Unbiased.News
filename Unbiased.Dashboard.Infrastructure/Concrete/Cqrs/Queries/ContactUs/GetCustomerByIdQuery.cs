using MediatR;
using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.ContactUs
{
    /// <summary>
    /// Query record for retrieving a specific customer contact by its unique identifier.
    /// </summary>
    /// <param name="Id">The unique identifier of the customer contact to retrieve.</param>
    public record GetCustomerByIdQuery(int Id) : IRequest<Contact>;
}