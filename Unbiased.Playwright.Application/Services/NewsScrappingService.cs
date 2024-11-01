using System.Text;
using Unbiased.Playwright.Application.Interfaces;
using Microsoft.Playwright;

namespace Unbiased.Playwright.Application.Services
{
    public class NewsScrappingService : INewsScrappingService
    {
        public async Task<bool> GetNewsByWebsiteScrapping(string websiteUrl, string scriptClass)
        {
            var st = ".category__list__item.son-dakika-haberleri h2 a";
            using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            var chromium = playwright.Chromium;
            var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.hurriyet.com.tr/son-dakika-haberleri/", new PageGotoOptions
            {
                Timeout = 60000,
                WaitUntil = WaitUntilState.DOMContentLoaded
            });
            var titles = await page.Locator(scriptClass).AllAsync();
            var list = new List<string>();
            foreach (var titleElement in titles)
            {
                await titleElement.ClickAsync();
                var titleOfNewsElement = await page.WaitForSelectorAsync(".news-detail-title");
                var detailOfNews = await page.Locator("p").AllAsync();
                var build = new StringBuilder();
                build.AppendLine(await titleOfNewsElement!.InnerTextAsync());
                foreach (var detail in detailOfNews)
                {
                    build.AppendLine(await detail!.InnerTextAsync());
                    build.AppendLine("\n");
                }
                build.AppendLine("------------------------------------------------------");
                Console.Write(build.ToString());
                await page.GoBackAsync();
            }

            return await Task.FromResult<bool>(true);
        }
    }
}
