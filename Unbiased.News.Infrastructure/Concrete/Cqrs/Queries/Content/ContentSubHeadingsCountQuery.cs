using MediatR;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content
{
    /// <summary>
    /// CQRS query that requests the total count of subheadings for a specific category.
    /// </summary>
    /// <param name="CategoryId">The ID of the category to count subheadings for.</param>
    public record ContentSubHeadingsCountQuery(int CategoryId) : IRequest<int>;
}
