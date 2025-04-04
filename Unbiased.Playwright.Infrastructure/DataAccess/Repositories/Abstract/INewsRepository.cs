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

        /// <summary>
        /// Retrieves all generated news items in the specified language.
        /// </summary>
        /// <param name="language">The language code of the news to retrieve (e.g., "EN" for English, "TR" for Turkish).</param>
        /// <returns>A collection of generated news entities in the specified language.</returns>
        Task<IEnumerable<GeneratedNews>> GetGeneratedNewsAsync(string language);

        /// <summary>
        /// Inserts a new question and its corresponding answer into the database.
        /// </summary>
        /// <param name="QuestionAndAnswer">The DTO containing the question, answer, and related metadata.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains a boolean value indicating whether the insert operation was successful.
        /// </returns>
        Task<bool> InsertQuestionAndAnswerAsync(QuestionAnswerDto QuestionAndAnswer);
    }
}
