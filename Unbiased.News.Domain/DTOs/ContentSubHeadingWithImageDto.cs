namespace Unbiased.News.Domain.DTOs
{

    /// <summary>
    /// Data Transfer Object for content subheadings with associated image information.
    /// </summary>
    public class ContentSubHeadingWithImageDto
    {
        /// <summary>
        /// Unique identifier of the content subheading.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the content subheading.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Indicates whether the content subheading is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Identifier of the related content category.
        /// </summary>
        public int ContentCategoryId { get; set; }

        /// <summary>
        /// Indicates whether the content has been processed.
        /// </summary>
        public bool IsProccessed { get; set; }

        /// <summary>
        /// Date and time when the content subheading was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Unique URL-friendly path for the content subheading.
        /// </summary>
        public string UniqUrlPath { get; set; }

        /// <summary>
        /// Path to the image associated with the content subheading.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Row number used for ordering in SQL queries (e.g., by CreatedTime).
        /// </summary>
        public int RowNum { get; set; }
    }


}
