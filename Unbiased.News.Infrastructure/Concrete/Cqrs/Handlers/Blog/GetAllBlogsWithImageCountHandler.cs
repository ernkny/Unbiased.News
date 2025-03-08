using MediatR;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Blog;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Blog
{
    public class GetAllBlogsWithImageCountHandler : IRequestHandler<GetAllBlogsWithImageCountQuery, int>
    {
        private readonly IBlogRepository _blogRepository;

        public GetAllBlogsWithImageCountHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<int> Handle(GetAllBlogsWithImageCountQuery request, CancellationToken cancellationToken)
        {
            return await _blogRepository.GetAllBlogsWithImageCountAsync(request.language, request.searchData);
        }
    }
}
