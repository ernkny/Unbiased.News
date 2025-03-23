namespace Unbiased.Playwright.Domain.Enums
{
    /// <summary>
    /// Enumerates the available image generation sources for the application.
    /// Used to specify which API or service should be used for generating images.
    /// </summary>
    public enum ImageGenerationSource
    {
        /// <summary>
        /// Use OpenAI's DALL-E model for image generation.
        /// </summary>
        Dalle,
        
        /// <summary>
        /// Use Freepik's API for image generation.
        /// </summary>
        Freepik
    }
}
