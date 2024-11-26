using Microsoft.Playwright;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrapping
{
    public  class SearchWithKeywordsMethod
    {
        public async Task<IEnumerable<string>> SearchWithKeywords(string keyword)
        {
            try
            {
                using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
                var chromium = playwright.Chromium;
                var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true,
                });

                var page = await browser.NewPageAsync();
                await page.GotoAsync("https://www.google.com", new PageGotoOptions
                {
                    Timeout = 60000,
                    WaitUntil = WaitUntilState.DOMContentLoaded
                });

                await page.FillAsync("textarea", keyword);
                await page.ClickAsync(".gNO89b");

                await page.WaitForSelectorAsync("a.WlydOe", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });

                var getlLastNews = await page.QuerySelectorAllAsync(".n0jPhd.ynAwRc.tNxQIb.nDgy9d");
                var titles = new List<string>();
                foreach (var l in getlLastNews)
                {
                    titles.Add(await l.TextContentAsync());
                }
                await page.CloseAsync();
                return titles;
            }
            catch (Exception exception)
            {

                throw exception.InnerException;
            }

        }
    }
}
