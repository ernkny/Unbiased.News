using MediatR;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Engine
{
    public record DeActivateOrActivateSearchCommand(string id):IRequest<bool>;
}
