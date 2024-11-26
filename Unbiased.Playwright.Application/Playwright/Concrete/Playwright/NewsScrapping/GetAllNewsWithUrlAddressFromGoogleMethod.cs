using Microsoft.Playwright;
using Unbiased.Playwright.Application.Dto.PlaywrightDto;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrapping
{
    public class GetAllNewsWithUrlAddressFromGoogleMethod
    {
        private IPlaywright _playwright;
        private IBrowser _browser;

        public async Task<List<SaveSearchUrlAndGuidDto>> GetAllNewsWithUrlAddressFromGoogle(string searchUrl)
        {
            var newsArticles = new List<SaveSearchUrlAndGuidDto>();
            try
            {
                _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
                _browser = await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });

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

                await page.WaitForSelectorAsync(".aqvwYd", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                await page.WaitForSelectorAsync("#i11", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                await page.ClickAsync("#i11");
                await Task.Delay(10000);

                var newsUrls = await page.QuerySelectorAllAsync(".jKHa4e");
                await page.WaitForSelectorAsync(".jKHa4e", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                var urlList = await Task.WhenAll(newsUrls.Take(10).Select(async url => await url.GetAttributeAsync("href")));

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

                        var shareButtons = await newPage.QuerySelectorAllAsync("[aria-label='Paylaş']");
                        var guid = Guid.NewGuid().ToString();

                        foreach (var button in shareButtons.Skip(1).Take(8))
                        {
                            await button.ClickAsync();
                            var copyButton = await newPage.WaitForSelectorAsync("[aria-label='Bağlantıyı kopyala']", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
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

                            var closeButton = await newPage.WaitForSelectorAsync("[aria-label='İletişim kutusunu kapat']", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
                            await closeButton.ClickAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"URL işlenirken bir hata oluştu: {url} - {ex.Message}");
                    }
                    finally
                    {
                        await newPage.CloseAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Genel bir hata oluştu: {ex.Message}");
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
