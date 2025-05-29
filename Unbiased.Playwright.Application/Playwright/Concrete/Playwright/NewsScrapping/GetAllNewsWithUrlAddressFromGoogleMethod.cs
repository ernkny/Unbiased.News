using Microsoft.Playwright;
using Unbiased.Playwright.Application.Dto.PlaywrightDto;
using Unbiased.Playwright.Domain.Enums;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrapping
{
    /// <summary>
    /// Method for retrieving news articles with URL addresses from Google.
    /// </summary>
    public class GetAllNewsWithUrlAddressFromGoogleMethod
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private readonly IServiceProvider _serviceProvider;

        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        public GetAllNewsWithUrlAddressFromGoogleMethod(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Retrieves a list of news articles with URL addresses from Google.
        /// </summary>
        /// <param name="searchUrl">The URL to search for news articles.</param>
        /// <returns>A list of <see cref="SaveSearchUrlAndGuidDto"/> objects containing the news article URLs and GUIDs.</returns>

        public async Task<List<SaveSearchUrlAndGuidDto>> GetAllNewsWithUrlAddressFromGoogle(string searchUrl,LanguageEnums languageEnum)
        {

            var closeDialog= languageEnum.ToString().Equals("TR", StringComparison.OrdinalIgnoreCase)? "İletişim kutusunu kapat": "Close dialog";
            var CopyLink = languageEnum.ToString().Equals("TR", StringComparison.OrdinalIgnoreCase)? "Bağlantıyı kopyala":"Copy link";
            var Share = languageEnum.ToString().Equals("TR", StringComparison.OrdinalIgnoreCase)? "Paylaş" : "Share";
            var newsArticles = new List<SaveSearchUrlAndGuidDto>();
            try
            {
                _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
                _browser = await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });

                if (_browser == null)
                {
                    throw new InvalidOperationException("Failed to launch browser");
                }

                var page = await _browser.NewPageAsync();
                await page.GotoAsync(searchUrl, new PageGotoOptions
                {
                    Timeout = 60000,
                    WaitUntil = WaitUntilState.DOMContentLoaded
                });
                await Task.Delay(5000);

                var newsUrls = await page.QuerySelectorAllAsync(".jKHa4e");
                await page.WaitForSelectorAsync(".jKHa4e", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                var urlList = await Task.WhenAll(newsUrls.Take(8).Select(async url => await url.GetAttributeAsync("href")));

                foreach (var url in urlList)
                {
                    var newPage = await _browser.NewPageAsync();
                    try
                    {
                        await newPage.GotoAsync($"https://news.google.com/{url}", new PageGotoOptions
                        {
                            Timeout = 60000,
                            WaitUntil = WaitUntilState.DOMContentLoaded
                        });

                        var shareButtons = await newPage.QuerySelectorAllAsync($"[aria-label='{Share}']");
                        var guid = Guid.NewGuid().ToString();

                        foreach (var button in shareButtons.Skip(1).Take(8))
                        {
                            await button.ClickAsync();
                            var copyButton = await newPage.WaitForSelectorAsync($"[aria-label='{CopyLink}']", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                            await copyButton.ClickAsync();
                            var clipboardText = await newPage.EvaluateAsync<string>("navigator.clipboard.readText();");

                            if (clipboardText != null)
                            {
                                newsArticles.Add(new SaveSearchUrlAndGuidDto
                                {
                                    MatchId = guid,
                                    Url = clipboardText
                                });
                            }
                            else
                            {
                                Console.WriteLine($"Failed to retrieve clipboard text for URL: {url}");
                            }

                            var closeButton = await newPage.WaitForSelectorAsync($"[data-mdc-dialog-action='close']", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                            await closeButton.ClickAsync();
                        }
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
                    finally
                    {
                        await newPage.CloseAsync();
                    }
                }
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
            finally
            {
                if (_browser != null)
                {
                    await _browser.CloseAsync();
                }
                if (_playwright != null)
                {
                    _playwright.Dispose();
                }
            }

            return newsArticles;
        }
    }
}
