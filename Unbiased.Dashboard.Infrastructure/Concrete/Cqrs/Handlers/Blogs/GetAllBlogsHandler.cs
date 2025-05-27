using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Blogs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Blogs
{
    /// <summary>
    /// Handler for processing GetAllBlogsQuery requests to retrieve all blogs with pagination support.
    /// </summary>
    public class GetAllBlogsHandler : IRequestHandler<GetAllBlogsQuery, IEnumerable<BlogDto>>
    {
        private readonly  IBlogRepository _blogRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllBlogsHandler class.
        /// </summary>
        /// <param name="blogRepository">The blog repository for data access operations.</param>
        public GetAllBlogsHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        /// <summary>
        /// Handles the GetAllBlogsQuery request and returns a collection of blog DTOs.
        /// </summary>
        /// <param name="request">The query request containing blog request parameters.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of blog DTOs.</returns>
        public async Task<IEnumerable<BlogDto>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
        {
            return await _blogRepository.GetAllBlogsAsync(request.blogRequestDto,request.blogRequestDto.PageNumber, request.blogRequestDto.PageSize);
        }
    }
}
