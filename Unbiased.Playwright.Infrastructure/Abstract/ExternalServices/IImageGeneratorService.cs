namespace Unbiased.Playwright.Infrastructure.Abstract.ExternalServices
{
    /// <summary>
    /// Interface for image generation services that provide functionality to generate images based on text prompts.
    /// Used by different image generation service implementations such as DALL-E or Freepik.
    /// </summary>
    public interface IImageGeneratorService
    {
        /// <summary>
        /// Generates an image URL based on the provided text prompt.
        /// </summary>
        /// <param name="prompt">The text prompt describing the image to generate.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A task representing the asynchronous operation, containing the URL of the generated image or null if unsuccessful.</returns>
        Task<string?> GenerateImageUrlAsync(string prompt, CancellationToken cancellationToken);
    }
}
