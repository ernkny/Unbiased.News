using MediatR;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries
{
    /// <summary>
    /// Represents a query to retrieve all news items that have not been included in a process.
    /// </summary>
    public record GetAllNewsByNotIncludedProcessQuery():IRequest<IEnumerable<News>>;
}
