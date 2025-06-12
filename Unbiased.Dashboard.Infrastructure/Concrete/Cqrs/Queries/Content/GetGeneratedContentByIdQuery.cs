using MediatR;
using Unbiased.Dashboard.Domain.Dto_s.Content;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Content
{
    /// <summary>
    /// Represents a query to retrieve all content categories from the database.
    /// </summary>
    /// <param name="Id"></param>
    public record GetGeneratedContentByIdQuery(int Id):IRequest<GeneratedContentDto>;
}
