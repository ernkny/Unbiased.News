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

        public string Description { get; set; }
    }
}
