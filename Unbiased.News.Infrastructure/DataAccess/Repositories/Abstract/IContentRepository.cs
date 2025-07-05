using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Repository interface for content-related database operations.
    /// Provides methods to retrieve and manage content data including daily content, horoscopes, and subheadings.
    /// </summary>
    public interface IContentRepository
    {
        /// <summary>
        /// Retrieves the most recent content entry from the database.
        /// </summary>
        /// <returns>The latest content object.</returns>
        Task<Contents> GetLastContentAsync();

        /// <summary>
        /// Retrieves the most recent daily horoscope information for all zodiac signs.
        /// </summary>
        /// <returns>A collection of daily horoscope details.</returns>
        Task<IEnumerable<HoroscopeDailyDetail>> GetDailyLastHoroscopesAsync();

        /// <summary>
        /// Retrieves a paginated collection of content subheadings for a specific category.
        /// </summary>
        /// <param name="categoryId">The ID of the category to retrieve subheadings for.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <returns>A collection of content subheadings for the specified category.</returns>
        Task<IEnumerable<ContentSubHeading>> ContentSubHeadingsAsync(int categoryId,int pageNumber);

        /// <summary>
        ///  Retrieves all content subheadings with associated images for the home page.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        Task<IEnumerable<ContentSubHeadingWithImageDto>> GetAllContentWithImageForHomePageAsync(string language);

        /// <summary>
        /// Retrieves generated content details by its unique URL.
        /// </summary>
        /// <param name="uniqUrl">The unique URL identifier for the content.</param>
        /// <returns>The content details associated with the URL.</returns>
        Task<GeneratedContentDto> GetGeneratedContentByUrlAsync(string uniqUrl,string language);

        /// <summary>
        /// Gets the total count of subheadings available for a specific category.
        /// </summary>
        /// <param name="categoryId">The ID of the category to count subheadings for.</param>
        /// <returns>The total number of subheadings in the category.</returns>
        Task<int> ContentSubHeadingsCountAsync(int categoryId);

        /// <summary>
        ///  Retrieves all content entries for the sitemap in a specific language.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        Task<IEnumerable<SitemapContentModel>> GetAllContentsForSiteMap(string language);
    }
}
