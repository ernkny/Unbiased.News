using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Represents a query to retrieve top categories with their generated news for the home page.
    /// </summary>
    /// <param name="language">The language code to filter categories by.</param>
    public record GetHomePageTopCategoriesGeneratedNewsQuery(string language):IRequest<IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>>;
}
