using MediatR;
using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.ContactUs
{
    public record GetCustomerByIdQuery(int Id) : IRequest<Contact>;
} 