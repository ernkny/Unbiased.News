using MediatR;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Content
{
    /// <summary>
    /// Handler class for processing ContentSubHeadingsCountQuery requests.
    /// Returns the total count of subheadings for a specified category.
    /// </summary>
    public class ContentSubHeadingsCountHandler : IRequestHandler<ContentSubHeadingsCountQuery, int>
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        /// Initializes a new instance of the ContentSubHeadingsCountHandler class.
        /// </summary>
        /// <param name="contentRepository">The content repository for accessing content data.</param>
        public ContentSubHeadingsCountHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        /// Handles the ContentSubHeadingsCountQuery request by retrieving the 
        /// total count of subheadings for the specified category.
        /// </summary>
        /// <param name="request">The request containing the category ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The total count of subheadings for the category.</returns>
        public async Task<int> Handle(ContentSubHeadingsCountQuery request, CancellationToken cancellationToken)
        {
            return await _contentRepository.ContentSubHeadingsCountAsync(request.CategoryId);
        }
    }
}
