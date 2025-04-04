using MediatR;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handles the GetAllActiveUrlsForSearchQuery by retrieving all active URLs for search from the repository.
    /// </summary>
    public class GetAllActiveUrlsForSearchHandler : IRequestHandler<GetAllActiveUrlsForSearchQuery, IEnumerable<ActiveUrlsForSearchDto>>
    {
        /// <summary>
        /// The search URL repository instance used to perform the operation.
        /// </summary>
        private readonly ISearchUrlRepository _searchUrlRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllActiveUrlsForSearchHandler"/> class.
        /// </summary>
        /// <param name="searchUrlRepository">The search URL repository to use for retrieving active URLs.</param>
        public GetAllActiveUrlsForSearchHandler(ISearchUrlRepository searchUrlRepository)
        {
            _searchUrlRepository = searchUrlRepository;
        }

        /// <summary>
        /// Handles the GetAllActiveUrlsForSearchQuery by retrieving all active URLs for search from the repository.
        /// </summary>
        /// <param name="request">The GetAllActiveUrlsForSearchQuery to handle.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning a collection of active URLs for search.</returns>
        public async Task<IEnumerable<ActiveUrlsForSearchDto>> Handle(GetAllActiveUrlsForSearchQuery request, CancellationToken cancellationToken)
        {
            return await _searchUrlRepository.GetAllActiveUrlsForSearchAsync();
        }
    }
}
