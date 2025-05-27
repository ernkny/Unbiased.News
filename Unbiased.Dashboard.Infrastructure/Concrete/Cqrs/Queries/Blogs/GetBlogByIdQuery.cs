using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Blogs
{
    /// <summary>
    /// Query record for retrieving a specific blog by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the blog to retrieve.</param>
    public record GetBlogByIdQuery(string id):IRequest<BlogDto>;
}
