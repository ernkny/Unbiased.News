using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Blogs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Blogs
{
    /// <summary>
    /// Handler for processing GetBlogByIdQuery requests to retrieve a specific blog by its identifier.
    /// </summary>
    public class GetBlogByIdHandler : IRequestHandler<GetBlogByIdQuery, BlogDto>
    {
        private readonly  IBlogRepository _blogRepository;

        /// <summary>
        /// Initializes a new instance of the GetBlogByIdHandler class.
        /// </summary>
        /// <param name="blogRepository">The blog repository for data access operations.</param>
        public GetBlogByIdHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        /// <summary>
        /// Handles the GetBlogByIdQuery request and returns the specified blog with its image.
        /// </summary>
        /// <param name="request">The query request containing the blog ID to retrieve.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing the blog DTO with image.</returns>
        public async Task<BlogDto> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
        {
           return await _blogRepository.GetBlogByIdWithImageAsync(request.id);
        }
    }
}
