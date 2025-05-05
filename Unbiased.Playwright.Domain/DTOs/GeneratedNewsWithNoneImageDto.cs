namespace Unbiased.Playwright.Domain.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for generated news with no image.
    /// </summary>
    public class GeneratedNewsWithNoneImageDto
    {
        /// <summary>
        /// /// Unique identifier for the generated news item.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Title of the generated news item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Path to the image associated with the generated news item.
        /// </summary>
        public string ImagePrompt { get; set; }

        /// <summary>
        /// Path to the image associated with the generated news item.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Indicates whether the image is a manual image.
        /// </summary>
        public bool IsManuelImage { get; set; }
    }
}
