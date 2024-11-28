namespace Unbiased.Playwright.Domain.DTOs
{
    /// <summary>
    /// Data transfer object for news extraction.
    /// </summary>
    public class NewsExtractDto
    {
        /// <summary>
        /// Gets or sets the title of the news.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the detailed content of the news.
        /// </summary>
        public string Detail { get; set; }
    }
}
