namespace Unbiased.Playwright.Domain.DTOs
{
    /// <summary>
    /// Data transfer object for retrieving news without images.
    /// </summary>
    public class GetNewsWithoutImageDto
    {
        /// <summary>
        /// Gets or sets the match identifier.
        /// </summary>
        public string MatchId { get; set; }

        /// <summary>
        /// Gets or sets the title of the news.
        /// </summary>
        public string Title { get; set; }
    }
}
