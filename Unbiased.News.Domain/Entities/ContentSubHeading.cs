namespace Unbiased.News.Domain.Entities
{
    /// <summary>
    /// Entity class representing a subheading within content categories.
    /// Subheadings organize content under specific categories and have their own unique URLs.
    /// </summary>
    public class ContentSubHeading
    {
        /// <summary>
        /// Gets or sets the unique identifier for the subheading.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title text of the subheading.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the subheading is currently active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the content category this subheading belongs to.
        /// </summary>
        public int ContentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the subheading has been processed.
        /// </summary>
        public bool IsProccessed { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the subheading was created.
        /// </summary>
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the ImagePath.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the unique URL path used to access this subheading's content.
        /// </summary>
        public string? UniqUrlPath { get; set; }
    }
}
