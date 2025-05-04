using MediatR;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handler class for processing GetAllContentCategoriesQuery requests.
    /// Retrieves all active content categories from the repository.
    /// </summary>
    public class GetAllContentCategoriesHandler : IRequestHandler<GetAllContentCategoriesQuery, IEnumerable<ContentCategory>>
    {
        private readonly  IContentRepository _contentRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllContentCategoriesHandler class.
        /// </summary>
        /// <param name="contentRepository">The content repository for accessing content data.</param>
        public GetAllContentCategoriesHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        /// Handles the GetAllContentCategoriesQuery request by retrieving
        /// all content categories from the repository.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of all content categories.</returns>
        public async Task<IEnumerable<ContentCategory>> Handle(GetAllContentCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _contentRepository.GetAllContentCategories(); 
        }
    }
}
