using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Defines the interface for a news repository.
    /// </summary>
    public interface INewsRepository
    {
        /// <summary>
        /// Retrieves all generated news asynchronously.
        /// </summary>
        /// <param name="language">The language code to filter news by.</param>
        /// <returns>A collection of generated news.</returns>
        Task<IEnumerable<GeneratedNew>> GetAllGeneratedNewsAsync(string language);

        /// <summary>
        /// Retrieves all generated news with images asynchronously.
        /// </summary>
        /// <param name="categoryId">The category ID to filter news by.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="language">The language code to filter news by.</param>
        /// <param name="title">Optional title filter for searching news.</param>
        /// <returns>A collection of generated news with images.</returns>
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync(int categoryId, int pageNumber, string language, string? title);
        
        /// <summary>
        /// Gets the count of all generated news items with images for a specific category.
        /// </summary>
        /// <param name="categoryId">The category ID to count news items from.</param>
        /// <param name="title">Optional title filter for counting matching news.</param>
        /// <returns>The count of news items matching the criteria.</returns>
        Task<int> GetAllGeneratedNewsWithImageCountAsync(int categoryId, string? title);

        /// <summary>
        /// Retrieves a specific news item by its unique URL with associated image.
        /// </summary>
        /// <param name="uniqUrlPath">The unique URL path of the news item to retrieve.</param>
        /// <returns>The requested news item with its associated image.</returns>
        Task<GenerateNewsWithImageDto> GetGeneratedNewsByUniqUrlWithImageAsync(string uniqUrlPath);
        
        /// <summary>
        /// Retrieves a specific news item by its ID with associated image.
        /// </summary>
        /// <param name="id">The unique identifier of the news item to retrieve.</param>
        /// <returns>The requested news item with its associated image.</returns>
        Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdWithImageAsync(string id);
        
        /// <summary>
        /// Retrieves banner news items for a specific category and language.
        /// </summary>
        /// <param name="categoryId">The category ID to retrieve banner news from.</param>
        /// <param name="language">The language of the news items to retrieve.</param>
        /// <returns>A collection of banner news items with images.</returns>
        Task<IEnumerable<GenerateNewsWithImageDto>> GetBannerGeneratedNewsWithImageAsync(int categoryId, string language);
        
        /// <summary>
        /// Retrieves the top latest news items from a specific category, excluding the news item with the provided unique URL.
        /// </summary>
        /// <param name="categoryId">The category ID to retrieve news from.</param>
        /// <param name="uniqUrlPath">The unique URL path of the news item to exclude from results.</param>
        /// <param name="language">The language of the news items to retrieve.</param>
        /// <returns>A collection of the latest top news items from the specified category.</returns>
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllLastTopGeneratedNewsWithCategoryIdForDetailAsync(int categoryId, string uniqUrlPath, string language);
        
        /// <summary>
        /// Retrieves questions and answers related to a specific match.
        /// </summary>
        /// <param name="MatchId">The unique identifier of the match to retrieve Q&A for.</param>
        /// <returns>A collection of questions and answers for the specified match.</returns>
        Task<IEnumerable<QuestionAnswerDto>> GetAllQuestionsAndAnswerWithMatchIdAsync(string MatchId);
        
        /// <summary>
        /// Retrieves statistical information about all news items in the system.
        /// </summary>
        /// <returns>Statistical data about news items.</returns>
        Task<StatisticsDto> GetNewsStatisticsAsync();
    }
}
