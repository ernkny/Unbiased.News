using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Interface for managing search URLs in the database.
    /// </summary>
    public interface ISearchUrlRepository
    {
      
        /// <summary>
        /// Retrieves all active keywords for search.
        /// </summary>
        /// <returns>A task containing a collection of active urls.</returns>
        Task<IEnumerable<ActiveUrlsForSearchDto>> GetAllActiveUrlsForSearchAsync();

        /// <summary>
        /// Updates the next run time for all search URLs.
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateAllSearhcUrlNextRunTimeAsync();

    }
}
