using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Blogs
{
    /// <summary>
    /// Query record for retrieving all blogs with pagination and filtering support.
    /// </summary>
    /// <param name="blogRequestDto">The request DTO containing pagination and filtering parameters.</param>
    public record GetAllBlogsQuery(BlogRequestDto blogRequestDto) :IRequest<IEnumerable<BlogDto>>;
}
