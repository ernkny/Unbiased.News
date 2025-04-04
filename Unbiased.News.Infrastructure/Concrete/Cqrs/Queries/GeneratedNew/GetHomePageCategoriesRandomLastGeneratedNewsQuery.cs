using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Represents a query to retrieve random recent news items grouped by categories for the home page.
    /// </summary>
    /// <param name="language">The language code to filter categories by.</param>
    public record GetHomePageCategoriesRandomLastGeneratedNewsQuery(string language):IRequest<IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>>;
}
