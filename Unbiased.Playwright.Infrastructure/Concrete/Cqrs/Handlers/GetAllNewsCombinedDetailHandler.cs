using MediatR;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handles the GetAllNewsCombinedDetailsQuery by retrieving all news items with combined details from the repository.
    /// </summary>
    public class GetAllNewsCombinedDetailHandler : IRequestHandler<GetAllNewsCombinedDetailsQuery, IEnumerable<GeneratedNewsDto>>
    {
        // The news repository instance used to perform the operation.
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllNewsCombinedDetailHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository to use for retrieving news items.</param>
        public GetAllNewsCombinedDetailHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the GetAllNewsCombinedDetailsQuery by retrieving all news items with combined details from the repository.
        /// </summary>
        /// <param name="request">The GetAllNewsCombinedDetailsQuery to handle.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning a collection of news items with combined details.</returns>
        public async Task<IEnumerable<GeneratedNewsDto>> Handle(GetAllNewsCombinedDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllCombinedDetailsAsync();
        }
    }
}
