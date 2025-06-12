namespace Unbiased.Dashboard.Domain.Dto_s.Content
{
    /// <summary>
    ///  Represents a category of content within the system.
    /// </summary>
    public class ContentCategories
    {
        /// <summary>
        ///  Gets or sets the unique identifier for the content category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the content category.
        /// </summary>
        public string CategoryName { get; set; }
    }
}
