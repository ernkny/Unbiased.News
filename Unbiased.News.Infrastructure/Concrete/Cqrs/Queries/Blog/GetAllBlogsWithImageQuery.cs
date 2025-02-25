using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Blog
{
    public record GetAllBlogsWithImageQuery(int pageNumber,string? searchData):IRequest<IEnumerable<BlogWithImageDto>>;
}
