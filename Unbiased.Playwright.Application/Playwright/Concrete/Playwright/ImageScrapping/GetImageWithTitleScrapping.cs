using Microsoft.Playwright;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.ImageScrapping
{
    /// <summary>
    /// A class for scraping images from Google based on a given title.
    /// </summary>
    public static class GetImageWithTitleScrapping
    {
        /// <summary>
        ///  Retrieves an image URL from Google Images based on the provided title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static async Task<string> GetImageWithTitle(string title,IPlaywright playwright, IEventAndActivityLog _eventAndActivityLog)
        {
            try
            {
                var chromium = playwright.Firefox;
                var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true,

                });
                var page = await browser.NewPageAsync();
                await page.GotoAsync("https://images.google.com", new PageGotoOptions
                {
                    Timeout = 60000,
                    WaitUntil = WaitUntilState.DOMContentLoaded
                });
                await page.FillAsync("textarea", title);
                await page.PressAsync("textarea[name='q']", "Enter");

                await page.WaitForSelectorAsync("img.YQ4gaf", new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 10000
                });
                await page.ClickAsync(".H8Rx8c img.YQ4gaf");

                var detailedImg = await page.WaitForSelectorAsync("img.sFlh5c.FyHeAf.iPVvYb", new PageWaitForSelectorOptions
                {
                    Timeout = 10000
                });
                var imageUrl = await detailedImg.GetAttributeAsync("src");
                return imageUrl is not null ? imageUrl : string.Empty;


            }
            catch (Exception ex)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = typeof(GetImageWithTitleScrapping).FullName,
                    EventSeverity = "Error",
                    Message = $"{ex.Message} - {ex.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
               return string.Empty;
            }
        }
    }
}
