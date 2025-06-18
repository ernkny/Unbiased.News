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
    private readonly IEventAndActivityLog _eventAndActivityLog;

    /// <summary>
    /// Initializes a new instance of the GetNewsWithGuidMethod class.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="eventAndActivityLog"></param>
    public GetNewsWithGuidMethod(IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
    {
        _serviceProvider = serviceProvider;
        _eventAndActivityLog = eventAndActivityLog;
    }

    /// <summary>
    /// Retrieves news articles with GUIDs from the provided URL and GUID pairs.
    /// </summary>
    /// <param name="urlAndGuidPairs">List of URL and GUID pairs.</param>
    /// <returns>List of news articles.</returns>
    public async Task<List<News>> GetNewsWithGuid(List<SaveSearchUrlAndGuidDto> urlAndGuidPairs)
    {
        var newsArticles = new ConcurrentBag<News>();

        _playwright ??= await Microsoft.Playwright.Playwright.CreateAsync();

        var browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        await Parallel.ForEachAsync(urlAndGuidPairs, async (urlPair, cancellationToken) =>
        {
            IBrowserContext context = null;
            IPage page = null;

            try
            {
                context = await browser.NewContextAsync();
                page = await context.NewPageAsync();

                var contentBuilder = new StringBuilder();

                await page.GotoAsync(urlPair.Url, new PageGotoOptions
                {
                    Timeout = 60000,
                    WaitUntil = WaitUntilState.DOMContentLoaded
                });

                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                var paragraphs = await page.QuerySelectorAllAsync("p:not([class])");

                foreach (var paragraph in paragraphs)
                {
                    try
                    {
                        var text = await paragraph.InnerTextAsync();
                        contentBuilder.AppendLine(text);
                    }
                    catch
                    {
                        // Paragraf hatası varsa atla
                    }
                }

                if (!string.IsNullOrWhiteSpace(contentBuilder.ToString()))
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
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
            }
            finally
            {
                if (page != null) await page.CloseAsync();
                if (context != null) await context.CloseAsync();
            }
        });

        await browser.CloseAsync();

        return newsArticles.ToList();
    }
}
