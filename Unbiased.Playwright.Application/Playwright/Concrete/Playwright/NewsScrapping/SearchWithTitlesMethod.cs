using Microsoft.Playwright;
using Unbiased.Playwright.Application.Dto.PlaywrightDto;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrapping
{
    /// <summary>
    /// This class is responsible for searching news titles on Google and returning the URLs and GUIDs of the search results.
    /// </summary>
    public class SearchWithTitlesMethod
    {
        /// <summary>
        /// Searches news titles on Google and returns the URLs and GUIDs of the search results.
        /// </summary>
        /// <param name="titleOfNews">A list of news titles to search for.</param>
        /// <returns>A list of SaveSearchUrlAndGuidDto objects containing the URLs and GUIDs of the search results.</returns>
        public async Task<List<SaveSearchUrlAndGuidDto>> SearchWithTitles(List<string> titleOfNews)
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
                var model = Enumerable.Empty<SaveSearchUrlAndGuidDto>().ToList();
                foreach (var item in titleOfNews)
                {

                    await page.GotoAsync(
                    "https://www.google.com/search?q=" + item, new PageGotoOptions
                    {
                        Timeout = 60000,
                        WaitUntil = WaitUntilState.DOMContentLoaded
                    });

                    var links = await page.QuerySelectorAllAsync("div.yuRUbf > div > span > a");
                    var guidForMatchedNews = Guid.NewGuid().ToString();
                    foreach (var link in links)
                    { 
                        var saveSearchUrlAndGuidDto = new SaveSearchUrlAndGuidDto();
                        var url = await link.GetAttributeAsync("href");
                        if (url != null)
                        {
                            saveSearchUrlAndGuidDto.Title = item;
                            saveSearchUrlAndGuidDto.MatchId = guidForMatchedNews;
                            saveSearchUrlAndGuidDto.Url = url;
                            saveSearchUrlAndGuidDto.IsProcessed = false;
                            saveSearchUrlAndGuidDto.CreatedTime = DateTime.UtcNow;
                            saveSearchUrlAndGuidDto.IsActive = true;
                            model.Add(saveSearchUrlAndGuidDto);
                        }
                    }
                }
                await page.CloseAsync();
                return model;
            }
            catch (Exception exception)
            {

                throw ;
            }
        }
    }
}
