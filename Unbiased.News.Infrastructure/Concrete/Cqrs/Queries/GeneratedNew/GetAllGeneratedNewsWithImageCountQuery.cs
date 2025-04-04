using MediatR;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Represents a query to count all generated news items with images for a specific category.
    /// </summary>
    /// <param name="categoryId">The category ID to count news items from.</param>
    /// <param name="title">Optional title filter for counting matching news.</param>
    public record GetAllGeneratedNewsWithImageCountQuery(int categoryId, string? title) : IRequest<int>;
}
