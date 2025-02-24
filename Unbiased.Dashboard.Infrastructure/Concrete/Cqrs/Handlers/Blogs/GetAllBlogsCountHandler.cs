using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Blogs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Blogs
{
    public class GetAllBlogsCountHandler : IRequestHandler<GetAllBlogsCountQuery, int>
    {
        private readonly IBlogRepository _blogRepository;

        public GetAllBlogsCountHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<int> Handle(GetAllBlogsCountQuery request, CancellationToken cancellationToken)
        {
            return await _blogRepository.GetAllBlogsCountAsync(request.blogRequestDto);
        }
    }
}
