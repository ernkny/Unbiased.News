using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Represents a query to retrieve a specific news item by its unique URL with associated image.
    /// </summary>
    /// <param name="UniqUrl">The unique URL of the news item to retrieve.</param>
    public record GetGeneratedNewsByUniqUrlWithImageQuery(string UniqUrl,string language):IRequest<GenerateNewsWithImageDto>;
}
