using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Blogs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Blogs
{
    public class GetBlogByIdHandler : IRequestHandler<GetBlogByIdQuery, BlogDto>
    {
        private readonly  IBlogRepository _blogRepository;

        public GetBlogByIdHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<BlogDto> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
        {
           return await _blogRepository.GetBlogByIdWithImageAsync(request.id);
        }
    }
}
