namespace Unbiased.Playwright.Domain.Entities
{
    /// <summary>
    /// Entity class representing a subheading within a content category.
    /// </summary>
    public class ContentSubHeading
    {
        /// <summary>
        /// Gets or sets the unique identifier for the content subheading.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the content category this subheading belongs to.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the name of the content subheading.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the subheading is active.
        /// </summary>
        public int ContentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the unique URL for the content subheading.
        /// </summary>
        public bool IsProccessed { get; set; }

        /// <summary>
        /// Gets or sets the unique URL for the content subheading.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the name of the content category this subheading belongs to.
        /// </summary>
        public string CategoryName { get; set; }
    }
}
