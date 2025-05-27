using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Blogs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Blogs
{
    /// <summary>
    /// Handler for processing GetAllBlogsCountQuery requests to retrieve the total count of blogs.
    /// </summary>
    public class GetAllBlogsCountHandler : IRequestHandler<GetAllBlogsCountQuery, int>
    {
        private readonly IBlogRepository _blogRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllBlogsCountHandler class.
        /// </summary>
        /// <param name="blogRepository">The blog repository for data access operations.</param>
        public GetAllBlogsCountHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        /// <summary>
        /// Handles the GetAllBlogsCountQuery request and returns the total count of blogs.
        /// </summary>
        /// <param name="request">The query request containing filtering parameters for counting blogs.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing the total count of blogs.</returns>
        public async Task<int> Handle(GetAllBlogsCountQuery request, CancellationToken cancellationToken)
        {
            return await _blogRepository.GetAllBlogsCountAsync(request.blogRequestDto);
        }
    }
}
