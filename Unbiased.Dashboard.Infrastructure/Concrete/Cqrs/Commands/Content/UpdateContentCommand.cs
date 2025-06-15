using MediatR;
using Unbiased.Dashboard.Domain.Dto_s.Content;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Content
{
    /// <summary>
    ///  Command to update content data in the system.
    /// </summary>
    /// <param name="UpdateAllContentDataRequest"></param>
    public record UpdateContentCommand(UpdateAllContentDataRequest UpdateAllContentDataRequest) : IRequest<bool>;
}
