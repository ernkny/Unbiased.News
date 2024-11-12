namespace Unbiased.Playwright.Application.Dto.PlaywrightDto
{
    public class SaveSearchUrlAndGuidDto
    {
        public string Title { get; set; }
        public string Url { get; set; }

        public string? MatchId { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
