namespace Unbiased.Playwright.Application.Interfaces
{
    public interface INewsScrappingService
    {

        Task<bool> GetNewsByWebsiteScrapping(string websiteUrl, string scriptClass);
    }
}
