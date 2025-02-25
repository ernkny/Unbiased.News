namespace Unbiased.News.Domain.DTOs
{
    public class BlogWithImageDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string MatchId { get; set; }
        public string Path { get; set; }
        public string UniqUrlPath { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
