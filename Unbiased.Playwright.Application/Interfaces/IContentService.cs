using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Application.Interfaces
{
    /// <summary>
    /// Service interface for content-related operations.
    /// Handles the generation and management of various content types including horoscopes and generated content.
    /// </summary>
    public interface IContentService
    {
        /// <summary>
        /// Adds a new daily horoscope to the system.
        /// </summary>
        /// <param name="horoscopeDetail">The horoscope details to add.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        Task<bool> AddDailyHoroscopeAsync(HoroscopeDailyDetail horoscopeDetail);
        
        /// <summary>
        /// Generates content for unprocessed subheadings.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        Task<bool> GenerateContentAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Generates new subheadings for content categories and saves them to the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        Task<bool> GenerateSubheadingsAndSaveAsync(CancellationToken cancellationToken);
    }
}
