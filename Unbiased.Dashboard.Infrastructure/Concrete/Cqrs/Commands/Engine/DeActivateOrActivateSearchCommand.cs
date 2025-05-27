using MediatR;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Engine
{
    /// <summary>
    /// Command record for deactivating or activating search functionality by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the search configuration to deactivate or activate.</param>
    public record DeActivateOrActivateSearchCommand(string id):IRequest<bool>;
}
