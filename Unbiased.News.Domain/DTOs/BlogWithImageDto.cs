namespace Unbiased.News.Domain.DTOs
{
    /// <summary>
    /// Data transfer object for blog posts with their associated images.
    /// </summary>
    public class BlogWithImageDto
    {
        /// <summary>
        /// Unique identifier for the blog post.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The title of the blog post.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The detailed content of the blog post.
        /// </summary>
        public string Detail { get; set; }
        
        /// <summary>
        /// The match identifier associated with the blog post, if applicable.
        /// </summary>
        public string MatchId { get; set; }
        
        /// <summary>
        /// The file path to the image associated with the blog post.
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// The unique URL path for accessing the blog post.
        /// </summary>
        public string UniqUrlPath { get; set; }
        
        /// <summary>
        /// The date and time when the blog post was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}
