namespace Unbiased.News.Domain.DTOs
{
    /// <summary>
    /// Data transfer object for displaying random recent news items grouped by categories on the home page.
    /// </summary>
    public class HomePageCategoriesRandomLastGeneratedNewsDto
    {
        /// <summary>
        /// The unique identifier of the category.
        /// </summary>
        public int CategoryId { get; set; }
        
        /// <summary>
        /// The name of the category.
        /// </summary>
        public string CategoryName { get; set; }
        
        /// <summary>
        /// The title of the news item.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The detailed content of the news item.
        /// </summary>
        public string Detail { get; set; }
        
        /// <summary>
        /// The unique identifier of the news item.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The file path to the image associated with the news item.
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// The date and time when the news item was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }
        
        /// <summary>
        /// The unique URL path for accessing the news item.
        /// </summary>
        public string UniqUrlPath { get; set; }
    }
}
