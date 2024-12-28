namespace Unbiased.Playwright.Domain.DTOs
{
    /// <summary>
    /// Data transfer object for inserting news.
    /// </summary>
    public class InsertNewsDto
    {
        /// <summary>
        /// Gets or sets the title of the news.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the detailed content of the news.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Gets or sets the category identifier for the news.
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the URL associated with the news.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Language of the news.
        /// </summary>
        public string Language { get; set; }
    }
}
