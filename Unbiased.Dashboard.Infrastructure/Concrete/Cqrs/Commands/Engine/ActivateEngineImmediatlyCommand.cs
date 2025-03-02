using MediatR;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Engine
{
    public record ActivateEngineImmediatlyCommand(string id):IRequest<bool>;
}
