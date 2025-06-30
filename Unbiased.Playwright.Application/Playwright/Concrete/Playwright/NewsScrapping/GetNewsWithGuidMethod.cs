using HtmlAgilityPack;
using Microsoft.Playwright;
using System.Collections.Concurrent;
using System.Net.Http;
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
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the GetNewsWithGuidMethod class.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="eventAndActivityLog"></param>
    public GetNewsWithGuidMethod(IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog, IPlaywright playwright, HttpClient httpClient)
    {
        _serviceProvider = serviceProvider;
        _eventAndActivityLog = eventAndActivityLog;
        _playwright = playwright;
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient), "HttpClient cannot be null. Please provide a valid HttpClient instance.");

    }

    /// <summary>
    /// Retrieves news articles with GUIDs from the provided URL and GUID pairs.
    /// </summary>
    /// <param name="urlAndGuidPairs">List of URL and GUID pairs.</param>
    /// <returns>List of news articles.</returns>
    public async Task<List<News>> GetNewsWithGuid(List<SaveSearchUrlAndGuidDto> urlAndGuidPairs)
    {
        var newsArticles = new ConcurrentBag<News>();

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

                int retries = 0;
                bool navigated = false;

                while (!navigated && retries < 3)
                {
                    try
                    {
                        await page.GotoAsync(urlPair.Url, new PageGotoOptions
                        {
                            Timeout = 60000,
                            WaitUntil = WaitUntilState.DOMContentLoaded
                        });

                        await page.WaitForLoadStateAsync(LoadState.Load, new PageWaitForLoadStateOptions
                        {
                            Timeout = 30000
                        });

                        navigated = true;
                    }
                    catch
                    {
                        retries++;
                        await Task.Delay(2000);
                    }
                }

                if (!navigated)
                {
                    await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                    {
                        EventType = this.GetType().FullName,
                        EventSeverity = "Warning",
                        Message = $"URL: {urlPair.Url} - Failed to navigate after attempts and url passed",
                        EventDate = DateTime.UtcNow
                    });


                    var paragraphs = await page.QuerySelectorAllAsync("p:not([class])");

                    if (paragraphs.Count == 0)
                        paragraphs = await page.QuerySelectorAllAsync("p");

                    foreach (var paragraph in paragraphs)
                    {
                        try
                        {
                            var text = await paragraph.InnerTextAsync();
                            contentBuilder.AppendLine(text);
                        }
                        catch
                        {
                            // Paragraf hatasını yut
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
            }
            catch (Exception ex)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"URL: {urlPair.Url} - {ex.Message} - {ex.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
            }
            finally
            {
                if (page != null) await page.CloseAsync();
                if (context != null) await context.CloseAsync();
            }
        });

        return newsArticles.ToList();
    }

    /// <summary>
    ///  Retrieves news articles with GUIDs from the provided URL and GUID pairs using a hybrid approach of HTML scraping and Playwright.
    /// </summary>
    /// <param name="urlAndGuidPairs"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    public async Task<List<News>> GetNewsWithGuidHybrid(List<SaveSearchUrlAndGuidDto> urlAndGuidPairs)
    {
        var newsArticles = new ConcurrentBag<News>();
        await using var browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });

        await Parallel.ForEachAsync(urlAndGuidPairs, new ParallelOptions
        {
            MaxDegreeOfParallelism = 4 // CPU dostu limit
        },
        async (urlPair, cancellationToken) =>
        {
            string contentFromHtml = null;

            try
            {
                var html = await _httpClient.GetStringAsync(urlPair.Url, cancellationToken);

                if (!string.IsNullOrEmpty(html))
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    var paragraphs = doc.DocumentNode
                        .SelectNodes("//p")
                        ?.Select(p => p.InnerText?.Trim())
                        .Where(t => !string.IsNullOrEmpty(t))
                        .ToList();

                    if (paragraphs != null && paragraphs.Count > 0)
                    {
                        contentFromHtml = string.Join(Environment.NewLine, paragraphs);
                    }
                }
            }
            catch
            {

            }

            if (string.IsNullOrWhiteSpace(contentFromHtml))
            {

                IBrowserContext context = null;
                IPage page = null;

                try
                {
                    context = await browser.NewContextAsync();
                    page = await context.NewPageAsync();

                    int retries = 0;
                    bool navigated = false;

                    while (!navigated && retries < 3)
                    {
                        try
                        {
                            await page.GotoAsync(urlPair.Url, new PageGotoOptions
                            {
                                Timeout = 10000,
                                WaitUntil = WaitUntilState.DOMContentLoaded
                            });

                            await page.WaitForLoadStateAsync(LoadState.Load, new PageWaitForLoadStateOptions
                            {
                                Timeout = 10000
                            });

                            navigated = true;
                        }
                        catch
                        {
                            retries++;
                            await Task.Delay(2000);
                        }
                    }

                    if (!navigated)
                    {
                        await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                        {
                            EventType = this.GetType().FullName,
                            EventSeverity = "Warning",
                            Message = $"URL: {urlPair.Url} - Failed to navigate after attempts and url passed",
                            EventDate = DateTime.UtcNow
                        });
                        return;
                    }

                    var paragraphs = await page.QuerySelectorAllAsync("p");

                    var sb = new StringBuilder();
                    foreach (var p in paragraphs)
                    {
                        try
                        {
                            var text = await p.InnerTextAsync();
                            if (!string.IsNullOrEmpty(text))
                                sb.AppendLine(text);
                        }
                        catch
                        {
                            // ignore
                        }
                    }

                    contentFromHtml = sb.ToString();
                }
                catch (Exception ex)
                {
                    await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                    {
                        EventType = this.GetType().FullName,
                        EventSeverity = "Error",
                        Message = $"URL: {urlPair.Url} - {ex.Message} - {ex.StackTrace}",
                        EventDate = DateTime.UtcNow
                    });
                }
                finally
                {
                    if (page != null)
                    {
                        try { await page.CloseAsync(); } catch { /* ignore */ }
                    }
                    if (context != null)
                    {
                        try { await context.CloseAsync(); } catch { /* ignore */ }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(contentFromHtml))
            {
                newsArticles.Add(new News
                {
                    CreatedTime = DateTime.UtcNow,
                    CreatedUser = "system",
                    Detail = contentFromHtml,
                    IsActive = true,
                    IsProcessed = false,
                    MatchId = urlPair.MatchId,
                    IsDeleted = false,
                    Title = urlPair.Title,
                    Url = urlPair.Url,
                });
            }
        });


        return newsArticles.ToList();
    }

}
