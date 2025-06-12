namespace Unbiased.Dashboard.Domain.Dto_s.Content
{
    /// <summary>
    /// Represents a content subheading item used in the dashboard.
    /// </summary>
    public class ContentSubheadingDto
    {
        /// <summary>
        /// The name of the content category this subheading belongs to.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// The language code of the content (e.g., "TR" or "EN").
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// The unique identifier of the subheading.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The title of the subheading.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The creation date and time of the subheading.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// The ID of the category to which this subheading belongs.
        /// </summary>
        public int ContentCategoryId { get; set; }

        /// <summary>
        /// Indicates whether the subheading has been processed (0 = No, 1 = Yes).
        /// </summary>
        public bool IsProccessed { get; set; }
    }
}
