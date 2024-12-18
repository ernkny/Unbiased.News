namespace Unbiased.Playwright.Domain.DTOs
{
    /// <summary>
    /// Represents a data transfer object for generated news.
    /// </summary>
    public class GeneratedNewsDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the match.
        /// </summary>
        public string MatchId { get; set; }

        /// <summary>
        /// Gets or sets the combined details of the news.
        /// </summary>
        public string CombinedDetails { get; set; }

        /// <summary>
        /// Gets or sets the CategorId of the news.
        /// </summary>
        public int CategoryId { get; set; }
    }
}
