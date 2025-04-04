using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Represents a query to retrieve all generated news items with images for a specific category,
    /// with pagination and language filtering support.
    /// </summary>
    /// <param name="categoryId">The category ID to filter news by.</param>
    /// <param name="pageNumber">The page number for pagination.</param>
    /// <param name="language">The language code to filter news by.</param>
    /// <param name="title">Optional title filter for searching news.</param>
    public record GetAllGeneratedNewsWithImageQuery(int categoryId, int pageNumber, string language, string? title) : IRequest<IEnumerable<GenerateNewsWithImageDto>>;
}
