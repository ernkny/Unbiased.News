using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Blog;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Blog
{
    public class GetBlogWithImageByUniqUrlHandler : IRequestHandler<GetBlogWithImageByUniqUrlQuery, BlogWithImageDto>
    {
        private readonly IBlogRepository _blogRepository;

        public GetBlogWithImageByUniqUrlHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<BlogWithImageDto> Handle(GetBlogWithImageByUniqUrlQuery request, CancellationToken cancellationToken)
        {
            return await _blogRepository.GetBlogWithImageByUniqUrlAsync(request.uniqUrl);
        }
    }
}
