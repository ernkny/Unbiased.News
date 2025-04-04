using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Represents a query to retrieve banner news items for a specific category and language.
    /// </summary>
    /// <param name="categoryId">The category ID to retrieve banner news from.</param>
    /// <param name="language">The language of the news items to retrieve.</param>
    public record GetBannerGeneratedNewsWithImageQuery(int categoryId,string language):IRequest<IEnumerable<GenerateNewsWithImageDto>>;
}
