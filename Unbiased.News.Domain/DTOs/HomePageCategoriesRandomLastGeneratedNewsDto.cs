namespace Unbiased.News.Domain.DTOs
{
    public class HomePageCategoriesRandomLastGeneratedNewsDto
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public Guid Id { get; set; }
        public string Path { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
