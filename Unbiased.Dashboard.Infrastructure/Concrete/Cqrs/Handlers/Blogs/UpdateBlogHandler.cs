using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Blogs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Blogs
{
    public class UpdateBlogHandler : IRequestHandler<UpdateBlogCommand, bool>
    {
        private readonly IBlogRepository _blogRepository;

        public UpdateBlogHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<bool> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {
            return await _blogRepository.UpdateBlogAsync(request.UpdateBlogDtoRequest);
        }
    }
}
