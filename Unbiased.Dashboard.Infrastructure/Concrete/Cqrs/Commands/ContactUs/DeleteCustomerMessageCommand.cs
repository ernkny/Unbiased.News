using MediatR;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.ContactUs
{
    public record DeleteCustomerMessageCommand(int Id) : IRequest<bool>;
} 