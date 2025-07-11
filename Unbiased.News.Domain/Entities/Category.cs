namespace Unbiased.News.Domain.Entities
{
    /// <summary>
    /// Represents a category entity.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Unique identifier for the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the category.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Order level of the category.
        /// </summary>
        public int OrderLevel { get; set; }

        /// <summary>
        ///  Description of the category.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///  Count of news items associated with this category.
        /// </summary>
        public int NewsCount { get; set; }

        /// <summary>
        ///  Language of the category content.
        /// </summary>
        public string Language { get; set; }
    }
}
