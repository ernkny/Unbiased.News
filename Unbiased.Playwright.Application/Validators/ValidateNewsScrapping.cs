using MediatR;
using Unbiased.Playwright.Application.Interfaces.Playwright;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Application.Validators
{
    /// <summary>
    /// Validates news scrapping operations.
    /// </summary>
    public class ValidateNewsScrapping : IValidateNewsScrapping
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the ValidateNewsScrapping class.
        /// </summary>
        /// <param name="newsRepository">The news repository instance.</param>
        /// <param name="mediator">The mediator instance.</param>
        public ValidateNewsScrapping(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Validates a URL for search with a given title asynchronously.
        /// </summary>
        /// <param name="title">The title to validate.</param>
        /// <returns>A boolean indicating whether the URL is valid.</returns>
        public async Task<bool> UB_sp_UrlValidateForSearchWithTitleAsync(string title)
        {
            return await _newsRepository.ValidateUrlForSearchWithTitleAsync(title);
        }
    }
}
