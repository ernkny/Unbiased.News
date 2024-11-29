using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries
{
    /// <summary>
    /// Represents a query to retrieve all news combined details.
    /// </summary>
    /// <remarks>This query is used to fetch all news items with their combined details.</remarks>
    public record GetAllNewsCombinedDetailsQuery:IRequest<IEnumerable<GeneratedNewsDto>>;
}
