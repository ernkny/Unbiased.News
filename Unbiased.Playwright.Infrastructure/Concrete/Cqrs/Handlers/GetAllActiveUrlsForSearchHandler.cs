using MediatR;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handles the GetAllActiveKeywordsForSearchQuery by retrieving all active keywords for search from the repository.
    /// </summary>
    public class GetAllActiveUrlsForSearchHandler : IRequestHandler<GetAllActiveUrlsForSearchQuery, IEnumerable<ActiveUrlsForSearchDto>>
    {
        /// <summary>
        /// The news repository instance used to perform the operation.
        /// </summary>
        private readonly ISearchUrlRepository _searchUrlRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllActiveUrlsForSearchHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository to use for retrieving active keywords.</param>
        public GetAllActiveUrlsForSearchHandler(ISearchUrlRepository searchUrlRepository)
        {
            _searchUrlRepository = searchUrlRepository;
        }

        /// <summary>
        /// Handles the GetAllActiveKeywordsForSearchQuery by retrieving all active keywords for search from the repository.
        /// </summary>
        /// <param name="request">The GetAllActiveKeywordsForSearchQuery to handle.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning a collection of active keywords.</returns>
        public async Task<IEnumerable<ActiveUrlsForSearchDto>> Handle(GetAllActiveUrlsForSearchQuery request, CancellationToken cancellationToken)
        {
            return await _searchUrlRepository.GetAllActiveUrlsForSearchAsync();
        }
    }
}
