using MediatR;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content
{
    /// <summary>
    /// CQRS query that requests a paginated list of subheadings for a specific category.
    /// </summary>
    /// <param name="CategoryId">The ID of the category to retrieve subheadings for.</param>
    /// <param name="PageNumber">The page number for pagination.</param>
    public record ContentSubHeadingsQuery(int CategoryId,int PageNumber) : IRequest<IEnumerable<ContentSubHeading>>;
}
