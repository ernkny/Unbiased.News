using MediatR;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Content
{
    /// <summary>
    /// Handler class for processing ContentSubHeadingsQuery requests.
    /// Retrieves a paginated list of subheadings for a specified category.
    /// </summary>
    public class ContentSubHeadingsHandler : IRequestHandler<ContentSubHeadingsQuery, IEnumerable<ContentSubHeading>>
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        /// Initializes a new instance of the ContentSubHeadingsHandler class.
        /// </summary>
        /// <param name="contentRepository">The content repository for accessing content data.</param>
        public ContentSubHeadingsHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        /// Handles the ContentSubHeadingsQuery request by retrieving a paginated
        /// list of subheadings for the specified category.
        /// </summary>
        /// <param name="request">The request containing the category ID and page number.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of content subheadings for the specified category.</returns>
        public async Task<IEnumerable<ContentSubHeading>> Handle(ContentSubHeadingsQuery request, CancellationToken cancellationToken)
        {
            return await _contentRepository.ContentSubHeadingsAsync(request.CategoryId,request.PageNumber);
        }
    }
}
