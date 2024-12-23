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

        Task<IEnumerable<GeneratedNewsWithNoneImageDto>> GenerateImagesAsyncWithNoneHasGenerated(CancellationToken cancellationToken);
        Task<bool> GenerateImagesWhenAllNewsHasGeneratedAsync(CancellationToken cancellationToken);
        Task<bool> GenerateNewsWithApiAsync(IEnumerable<GeneratedNewsDto> combinedNews, CancellationToken cancellationToken, GptApiExternalService externalServiceSend);
    }
}
