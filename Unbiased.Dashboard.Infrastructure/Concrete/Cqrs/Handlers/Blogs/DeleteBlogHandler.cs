using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Blogs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Blogs
{
    /// <summary>
    /// Handler for processing DeleteBlogCommand requests to delete a blog by its identifier.
    /// </summary>
    internal class DeleteBlogHandler : IRequestHandler<DeleteBlogCommand, bool>
    {
        private readonly IBlogRepository _blogRepository;

        /// <summary>
        /// Initializes a new instance of the DeleteBlogHandler class.
        /// </summary>
        /// <param name="blogRepository">The blog repository for data access operations.</param>
        public DeleteBlogHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        /// <summary>
        /// Handles the DeleteBlogCommand request and deletes the specified blog.
        /// </summary>
        /// <param name="request">The command request containing the blog ID to delete.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        public async Task<bool> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
           return await _blogRepository.DeleteBlogByIdAsync(request.id);
        }
    }
}
