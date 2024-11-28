using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Application.Interfaces.Playwright
{
    /// <summary>
    /// Interface for Playwright Scrapping Service
    /// </summary>
    public interface IPlaywrightScrappingService
    {
        /// <summary>
        /// Asynchronously scrapes news using Playwright
        /// </summary>
        /// <returns>A list of scraped news</returns>
        Task<List<News>> PlaywrightScrappingNewsAsync();

        /// <summary>
        /// Saves a list of news with a specified range
        /// </summary>
        /// <param name="listOfNews">The list of news to save</param>
        /// <returns>True if the operation is successful, false otherwise</returns>
        Task<bool> SaveAllNewsWithRangeAsync(List<News> listOfNews);

        /// <summary>
        /// Asynchronously scrapes news and adds them to the range
        /// </summary>
        /// <returns>True if the operation is successful, false otherwise</returns>
        Task<bool> PlaywrightScrappingNewsAndAddRangeNewsAsync();

        /// <summary>
        /// Scrapes news using Playwright
        /// </summary>
        /// <returns>A list of scraped news</returns>
        Task<List<News>> PlaywrightScrappingNews();

        /// <summary>
        /// Saves a list of news with a specified range
        /// </summary>
        /// <param name="listOfNews">The list of news to save</param>
        /// <returns>True if the operation is successful, false otherwise</returns>
        Task<bool> SaveAllNewsWithRange(List<News> listOfNews);

        /// <summary>
        /// Scrapes news and adds them to the range
        /// </summary>
        /// <returns>True if the operation is successful, false otherwise</returns>
        Task<bool> PlaywrightScrappingNewsAndAddRangeNews();

        /// <summary>
        /// Gets images for collected news
        /// </summary>
        /// <returns>True if the operation is successful, false otherwise</returns>
        Task<bool> GetImagesForCollectedNews();
    }
}
