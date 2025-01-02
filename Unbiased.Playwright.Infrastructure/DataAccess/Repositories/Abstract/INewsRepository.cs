using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Interface for news repository operations.
    /// </summary>
    public interface INewsRepository
    {
        /// <summary>
        /// Adds a new news item to the repository.
        /// </summary>
        /// <param name="addNewsDto">The news item to add.</param>
        /// <returns>A task containing the ID of the added news item.</returns>
        Task<Guid> AddNewsAsync(InsertNewsDto addNewsDto);

        /// <summary>
        /// Retrieves all news items that have not been processed.
        /// </summary>
        /// <returns>A task containing a collection of unprocessed news items.</returns>
        Task<IEnumerable<News>> GetAllNewsByNotIncludedProcessAsync();

        /// <summary>
        /// Adds a range of news items to the repository.
        /// </summary>
        /// <param name="news">The news items to add.</param>
        /// <returns>A task containing a boolean indicating whether the operation was successful.</returns>
        Task<bool> AddRangeAllNewsAsync(IEnumerable<News> news);

        /// <summary>
        /// Validates a URL for search with a given title.
        /// </summary>
        /// <param name="title">The title to validate.</param>
        /// <returns>A task containing a boolean indicating whether the URL is valid.</returns>
        Task<bool> ValidateUrlForSearchWithTitleAsync(string title);

        /// <summary>
        /// Retrieves all combined news details.
        /// </summary>
        /// <returns>A task containing a collection of combined news details.</returns>
        Task<IEnumerable<GeneratedNewsDto>> GetAllCombinedDetailsAsync();

        /// <summary>
        /// Adds an OpenAI response to the repository.
        /// </summary>
        /// <param name="responseBody">The response body to add.</param>
        /// <returns>A task containing a boolean indicating whether the operation was successful.</returns>
        Task<bool> AddOpenAiResponseAsync(string responseBody);

        /// <summary>
        /// Adds a generated news item to the repository.
        /// </summary>
        /// <param name="generatedNews">The generated news item to add.</param>
        /// <returns>A task containing a boolean indicating whether the operation was successful.</returns>
        Task<bool> AddGeneratedNews(News generatedNews);

        /// <summary>
        /// Updates the process value of a news item to true.
        /// </summary>
        /// <param name="matchId">The ID of the news item to update.</param>
        /// <returns>A task containing a boolean indicating whether the operation was successful.</returns>
        Task<bool> UpdateNewsProcessValueAsTrueAsync(string matchId);

        Task<IEnumerable<GeneratedNews>> GetGeneratedNewsAsync(string language);
    }
}
