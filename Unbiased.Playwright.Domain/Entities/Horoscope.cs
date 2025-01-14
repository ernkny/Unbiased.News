namespace Unbiased.Playwright.Domain.Entities
{
    public class Horoscope
    {
        public int Id { get; set; } 
        public string HoroscopeName { get; set; } 
        public DateTime? LastRunDate { get; set; } 
        public DateTime? NextRunDate { get; set; } 
        public string? Path { get; set; } 
    }
}
