using Microsoft.Playwright;
using System.Collections.Concurrent;
using System.Text;
using Unbiased.Playwright.Application.Dto.PlaywrightDto;
using Unbiased.Playwright.Domain.Entities;

/// <summary>
/// Class responsible for retrieving news articles with GUIDs.
/// </summary>
public class GetNewsWithGuidMethod
{
    private IPlaywright _playwright;

    /// <summary>
    /// Retrieves news articles with GUIDs from the provided URL and GUID pairs.
    /// </summary>
    /// <param name="urlAndGuidPairs">List of URL and GUID pairs.</param>
    /// <returns>List of news articles.</returns>

    public async Task<List<News>> GetNewsWithGuid(List<SaveSearchUrlAndGuidDto> urlAndGuidPairs)
    {
        var newsArticles = new ConcurrentBag<News>();
        if (_playwright == null)
        {
            _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        }
        var browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
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
                if (!string.IsNullOrEmpty(contentBuilder.ToString()))
                {
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
                }
                
                await page.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Someting went wrong: {ex.Message}");
            }
        });

        await browser.CloseAsync();

        return newsArticles.ToList();
    }
}
