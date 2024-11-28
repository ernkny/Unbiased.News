namespace Unbiased.Playwright.Application.Interfaces.Playwright
{
    /// <summary>
    /// Interface for validating news scrapping operations.
    /// </summary>
    public interface IValidateNewsScrapping
    {
        /// <summary>
        /// Validates a URL for search with a given title asynchronously.
        /// </summary>
        /// <param name="title">The title to search for.</param>
        /// <returns>A task that represents the asynchronous operation, containing a boolean indicating whether the URL is valid.</returns>
        Task<bool> UB_sp_UrlValidateForSearchWithTitleAsync(string title);
    }
}
