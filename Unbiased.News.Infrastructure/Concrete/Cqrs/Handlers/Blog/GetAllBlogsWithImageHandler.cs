using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Blog;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Blog
{
    public class GetAllBlogsWithImageHandler : IRequestHandler<GetAllBlogsWithImageQuery, IEnumerable<BlogWithImageDto>>
    {
        private readonly  IBlogRepository _blogRepository;

        public GetAllBlogsWithImageHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IEnumerable<BlogWithImageDto>> Handle(GetAllBlogsWithImageQuery request, CancellationToken cancellationToken)
        {
            return await _blogRepository.GetAllBlogsWithImageAsync(request.pageNumber, request.searchData);
        }
    }
}
