using Microsoft.Playwright;
using System.Collections.Concurrent;
using System.Text;
using Unbiased.Playwright.Application.Dto.PlaywrightDto;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

/// <summary>
/// Class responsible for retrieving news articles with GUIDs.
/// </summary>
public class GetNewsWithGuidMethod
{
    private IPlaywright _playwright;
    private readonly IServiceProvider _serviceProvider;
    private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

    public GetNewsWithGuidMethod(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

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
                        CreatedTime = DateTime.UtcNow,
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
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        });

        await browser.CloseAsync();

        return newsArticles.ToList();
    }
}
