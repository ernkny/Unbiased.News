using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.Concrete.ExternalServices;

namespace Unbiased.Playwright.Application.Interfaces
{
    /// <summary>
    /// Interface for news service operations.
    /// </summary>
    public interface INewsService
    {
        /// <summary>
        /// Adds a new news item asynchronously.
        /// </summary>
        /// <param name="addNewsDto">The DTO containing the news item data.</param>
        /// <returns>A task representing the asynchronous operation, returning the GUID of the added news item.</returns>
        Task<Guid> AddNewsAsync(InsertNewsDto addNewsDto);

        /// <summary>
        /// Sends news to API for generation asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning a boolean indicating success or failure.</returns>
        Task<bool> SendNewsToApiForGenerateAsync(CancellationToken cancellationToken);

        /// <summary>
        ///  Retrieves all news items that have not been processed yet.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<GeneratedNewsWithNoneImageDto>> GenerateImagesAsyncWithNoneHasGenerated(CancellationToken cancellationToken);

        /// <summary>
        ///  Retrieves all combined news details asynchronously.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> GenerateImagesWhenAllNewsHasGeneratedAsync(CancellationToken cancellationToken);

        /// <summary>
        ///  Generates news using an external API asynchronously.
        /// </summary>
        /// <param name="combinedNews"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="externalServiceSend"></param>
        /// <returns></returns>
        Task<bool> GenerateNewsWithApiAsync(IEnumerable<GeneratedNewsDto> combinedNews, CancellationToken cancellationToken, GptApiExternalService externalServiceSend);
    }
}
