using MediatR;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Content
{
    /// <summary>
    /// Represents a query to retrieve the count of all contents based on language, category, and processing status.
    /// </summary>
    /// <param name="Language"></param>
    /// <param name="CategoryId"></param>
    /// <param name="IsProcess"></param>
    public record GetAllContentsCountQuery(string Language, int? CategoryId, bool? IsProcess):IRequest<int>;

}
