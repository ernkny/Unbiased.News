using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface ISearchUrlRepository
    {
      
        /// <summary>
        /// Retrieves all active keywords for search.
        /// </summary>
        /// <returns>A task containing a collection of active urls.</returns>
        Task<IEnumerable<ActiveUrlsForSearchDto>> GetAllActiveUrlsForSearchAsync();

        Task<bool> UpdateAllSearhcUrlNextRunTimeAsync();

    }
}
