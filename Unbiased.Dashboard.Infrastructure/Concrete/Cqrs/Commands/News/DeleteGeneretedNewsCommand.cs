using MediatR;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.News
{
    public record DeleteGeneretedNewsCommand(string id):IRequest<bool>;
}
