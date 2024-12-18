using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries
{
    /// <summary>
    /// Represents a query to retrieve all active urls for search.
    /// </summary>
    /// <remarks>This query does not contain any parameters.</remarks>
    public record GetAllActiveUrlsForSearchQuery : IRequest<IEnumerable<ActiveUrlsForSearchDto>>;
}
