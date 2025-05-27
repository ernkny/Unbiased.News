using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Blogs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Blogs
{
    /// <summary>
    /// Handler for processing InsertBlogCommand requests to create a new blog.
    /// </summary>
    public class InsertBlogHandler : IRequestHandler<InsertBlogCommand, bool>
    {
        private readonly IBlogRepository _blogRepository;

        /// <summary>
        /// Initializes a new instance of the InsertBlogHandler class.
        /// </summary>
        /// <param name="blogRepository">The blog repository for data access operations.</param>
        public InsertBlogHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        /// <summary>
        /// Handles the InsertBlogCommand request and creates a new blog.
        /// </summary>
        /// <param name="request">The command request containing the blog data and user ID.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        public async Task<bool> Handle(InsertBlogCommand request, CancellationToken cancellationToken)
        {
           return await _blogRepository.InsertBlogAsync(request.InsertBlogDtoRequest,request.userId);
        }
    }
}
