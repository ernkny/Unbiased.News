using MediatR;
using Unbiased.Dashboard.Domain.Dto_s.Content;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Content
{
    /// <summary>
    /// Represents a query to retrieve all content categories from the database.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    /// <param name="Language"></param>
    /// <param name="CategoryId"></param>
    /// <param name="IsProcess"></param>
    public record GetAllContentSubheadingQuery(int PageNumber, int PageSize, string Language, int? CategoryId,bool? IsProcess) : IRequest<IEnumerable<ContentSubheadingDto>>;

}
