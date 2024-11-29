using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handles the ValidateUrlForSearchWithTitleQuery by validating a URL for search with a title.
    /// </summary>
    public class ValidateUrlForSearchWithTitleHandler : IRequestHandler<ValidateUrlForSearchWithTitleQuery, bool>
    {
        // The news repository instance used to perform the operation.
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the ValidateUrlForSearchWithTitleHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository to use for validating the URL.</param>
        public ValidateUrlForSearchWithTitleHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the ValidateUrlForSearchWithTitleQuery by validating a URL for search with a title.
        /// </summary>
        /// <param name="request">The ValidateUrlForSearchWithTitleQuery to handle.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning a boolean indicating whether the URL is valid.</returns>
        public async Task<bool> Handle(ValidateUrlForSearchWithTitleQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.ValidateUrlForSearchWithTitleAsync(request.title);
        }
    }
}
