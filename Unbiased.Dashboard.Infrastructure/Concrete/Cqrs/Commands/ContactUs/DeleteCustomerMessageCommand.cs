using MediatR;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.ContactUs
{
    /// <summary>
    /// Command record for deleting a customer message by its unique identifier.
    /// </summary>
    /// <param name="Id">The unique identifier of the customer message to delete.</param>
    public record DeleteCustomerMessageCommand(int Id) : IRequest<bool>;
} 