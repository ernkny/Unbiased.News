using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Blogs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Blogs
{
    /// <summary>
    /// Handler for processing UpdateBlogCommand requests to update an existing blog.
    /// </summary>
    public class UpdateBlogHandler : IRequestHandler<UpdateBlogCommand, bool>
    {
        private readonly IBlogRepository _blogRepository;

        /// <summary>
        /// Initializes a new instance of the UpdateBlogHandler class.
        /// </summary>
        /// <param name="blogRepository">The blog repository for data access operations.</param>
        public UpdateBlogHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        /// <summary>
        /// Handles the UpdateBlogCommand request and updates the specified blog.
        /// </summary>
        /// <param name="request">The command request containing the updated blog data.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        public async Task<bool> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {
            return await _blogRepository.UpdateBlogAsync(request.UpdateBlogDtoRequest);
        }
    }
}
