using MediatR;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries
{
    /// <summary>
    /// Represents a query to validate a URL for search with a title.
    /// </summary>
    /// <param name="title">The title to search for.</param>
    /// <returns>A boolean indicating whether the URL is valid for search.</returns>
    public record ValidateUrlForSearchWithTitleQuery(string title) : IRequest<bool>;

}
