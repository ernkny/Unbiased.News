namespace Unbiased.Playwright.Domain.DTOs
{
    public class ActiveUrlsForSearchDto
    {
        public string url { get; set; }
        public int categoryId { get; set; }
        public string Language { get; set; }

        public DateTime? LastUpdatedTime { get; set; }
        public DateTime NextRunTime { get; set; }
    }
}
