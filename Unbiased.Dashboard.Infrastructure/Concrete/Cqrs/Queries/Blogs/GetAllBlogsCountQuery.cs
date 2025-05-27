using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Blogs
{
    /// <summary>
    /// Query record for retrieving the total count of blogs based on filtering criteria.
    /// </summary>
    /// <param name="blogRequestDto">The request DTO containing filtering parameters for counting blogs.</param>
    public record GetAllBlogsCountQuery(BlogRequestDto blogRequestDto) : IRequest<int>;
}
