using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Blogs
{
    public record GetAllBlogsCountQuery(BlogRequestDto blogRequestDto) : IRequest<int>;
}
