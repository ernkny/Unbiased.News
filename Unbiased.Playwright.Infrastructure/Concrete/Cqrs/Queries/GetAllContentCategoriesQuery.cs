using MediatR;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries
{
    /// <summary>
    /// CQRS query that requests all content categories from the database.
    /// Used to retrieve a comprehensive list of all available content categories.
    /// </summary>
    public record GetAllContentCategoriesQuery:IRequest<IEnumerable<ContentCategory>>;
}
