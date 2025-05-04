namespace Unbiased.Playwright.Domain.DTOs
{
    /// <summary>
    /// Data transfer object for extracted news content from external sources or APIs.
    /// Contains the structured news content along with bias assessment information.
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

        /// <summary>
        /// Gets or sets the explanation of how the bias score was determined.
        /// Provides context and reasoning for the assigned bias score.
        /// </summary>
        public string BiasScoreExplanation { get; set; }

        /// <summary>
        /// Gets or sets the bias score of the news content.
        /// Indicates the level of bias detected in the news, typically on a scale of 0-100.
        /// </summary>
        public string BiasScore { get; set; }

        /// <summary>
        /// Gets or sets the promt of the news source to create Image
        /// </summary>
        public string ImagePrompt { get; set; }
    }
}
