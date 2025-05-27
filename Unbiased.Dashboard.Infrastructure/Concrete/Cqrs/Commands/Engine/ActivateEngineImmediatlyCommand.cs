using MediatR;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Engine
{
    /// <summary>
    /// Command record for activating the engine immediately by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the engine to activate immediately.</param>
    public record ActivateEngineImmediatlyCommand(string id):IRequest<bool>;
}
