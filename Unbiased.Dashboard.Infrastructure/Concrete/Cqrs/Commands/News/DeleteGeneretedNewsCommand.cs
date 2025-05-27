using MediatR;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.News
{
    /// <summary>
    /// Command record for deleting generated news by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the generated news to delete.</param>
    public record DeleteGeneretedNewsCommand(string id):IRequest<bool>;
}
