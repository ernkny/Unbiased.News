namespace Unbiased.News.Domain.DTOs
{
    /// <summary>
    ///  Represents a model for content in the sitemap.
    /// </summary>
    public class SitemapContentModel
    {
        /// <summary>
        ///  Gets or sets the unique identifier for the content.
        /// </summary>
        public string UniqUrlPath { get; set; }

        /// <summary>
        ///   Gets or sets the title of the content.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///  Gets or sets the description of the content.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        ///   Gets or sets the image path associated with the content.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the language of the content.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        ///  Gets or sets the category name of the content.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        ///  Gets or sets the author of the content.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the content is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }

}
