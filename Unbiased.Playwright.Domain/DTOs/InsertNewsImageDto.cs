namespace Unbiased.Playwright.Domain.DTOs
{
    /// <summary>
    /// Data transfer object for inserting news images.
    /// </summary>
    public class InsertNewsImageDto
    {
        /// <summary>
        /// Unique identifier for the match associated with the news image.
        /// </summary>
        public string MatchId { get; set; }

        /// <summary>
        /// Base64-encoded image data.
        /// </summary>
        public string filePath { get; set; }
    }
}
