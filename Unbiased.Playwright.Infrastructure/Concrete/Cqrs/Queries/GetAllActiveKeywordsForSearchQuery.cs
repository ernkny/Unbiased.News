using MediatR;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries
{
    /// <summary>
    /// Represents a query to retrieve all active keywords for search.
    /// </summary>
    /// <remarks>This query does not contain any parameters.</remarks>
    public record GetAllActiveKeywordsForSearchQuery : IRequest<IEnumerable<string>>;
}
