using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Blogs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Blogs
{
    public class GetAllBlogsHandler : IRequestHandler<GetAllBlogsQuery, IEnumerable<BlogDto>>
    {
        private readonly  IBlogRepository _blogRepository;

        public GetAllBlogsHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IEnumerable<BlogDto>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
        {
            return await _blogRepository.GetAllBlogsAsync(request.blogRequestDto,request.blogRequestDto.PageNumber, request.blogRequestDto.PageSize);
        }
    }
}
