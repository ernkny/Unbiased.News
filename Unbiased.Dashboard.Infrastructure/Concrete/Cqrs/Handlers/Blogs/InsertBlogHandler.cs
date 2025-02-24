using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Blogs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Blogs
{
    public class InsertBlogHandler : IRequestHandler<InsertBlogCommand, bool>
    {
        private readonly IBlogRepository _blogRepository;

        public InsertBlogHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<bool> Handle(InsertBlogCommand request, CancellationToken cancellationToken)
        {
           return await _blogRepository.InsertBlogAsync(request.InsertBlogDtoRequest,request.userId);
        }
    }
}
