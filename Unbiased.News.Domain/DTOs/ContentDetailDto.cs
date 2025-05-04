namespace Unbiased.News.Domain.DTOs
{
    /// <summary>
    /// Data transfer object that represents the detailed content information.
    /// Contains essential metadata about content such as ID, subtitle, image details, and hashtags.
    /// </summary>
    public class ContentDetailDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the content.
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// Gets or sets the subtitle or secondary heading of the content.
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Gets or sets the prompt text used to generate the content's image.
        /// </summary>
        public string ImagePrompt { get; set; }

        /// <summary>
        /// Gets or sets the hashtags associated with the content for categorization and search.
        /// </summary>
        public string Hashtags { get; set; }

        /// <summary>
        /// Gets or sets the file path or URL to the content's image.
        /// </summary>
        public string ImagePath { get; set; }
    }
}
