using Microsoft.Playwright;
using System.Collections.Concurrent;
using System.Text;
using Unbiased.Playwright.Application.Dto.PlaywrightDto;
using Unbiased.Playwright.Domain.Entities;

public class GetNewsWithGuidMethod
{
    private IPlaywright _playwright;

    public async Task<List<News>> GetNewsWithGuid(List<SaveSearchUrlAndGuidDto> urlAndGuidPairs)
    {
        var newsArticles = new ConcurrentBag<News>();
        if (_playwright == null)
        {
            _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        }
        var browser = await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        await Parallel.ForEachAsync(urlAndGuidPairs, async (urlPair, cancellationToken) =>
        {
            try
            {
                var page = await browser.NewPageAsync();
                var contentBuilder = new StringBuilder();
                await page.GotoAsync(urlPair.Url, new PageGotoOptions { Timeout = 60000, WaitUntil = WaitUntilState.DOMContentLoaded });
                var paragraphs = await page.QuerySelectorAllAsync("p:not([class])");
                foreach (var paragraph in paragraphs)
                {
                    contentBuilder.AppendLine(await paragraph.InnerTextAsync());
                }
                newsArticles.Add(new News
                {
                    CreatedTime = DateTime.Now,
                    CreatedUser = "system",
                    Detail = contentBuilder.ToString(),
                    IsActive = true,
                    IsProcessed = false,
                    MatchId = urlPair.MatchId,
                    IsDeleted = false,
                    Title = urlPair.Title,
                    Url = urlPair.Url,
                });
                await page.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"URL işlenirken bir hata oluştu: {urlPair.Url} - {ex.Message}");
            }
        });

        await browser.CloseAsync();

        return newsArticles.ToList();
    }
}
