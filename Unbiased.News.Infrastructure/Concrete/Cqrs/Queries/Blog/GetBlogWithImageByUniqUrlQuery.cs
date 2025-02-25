using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Blog
{
    public record GetBlogWithImageByUniqUrlQuery(string uniqUrl):IRequest<BlogWithImageDto>;
}
