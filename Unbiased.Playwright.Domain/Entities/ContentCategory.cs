namespace Unbiased.Playwright.Domain.Entities
{
    /// <summary>
    /// Entity class representing a content category.
    /// Categories are used to organize and group related content items.
    /// </summary>
    public class ContentCategory
    {
        /// <summary>
        /// Gets or sets the unique identifier for the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the category is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the language of the category content.
        /// </summary>
        public string Language { get; set; }
    }
}
