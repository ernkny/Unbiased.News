using MediatR;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Blog
{
    public record GetAllBlogsWithImageCountQuery(string? searchData):IRequest<int>;
}
