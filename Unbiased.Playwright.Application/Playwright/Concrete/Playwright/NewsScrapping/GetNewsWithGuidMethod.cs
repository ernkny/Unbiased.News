using Microsoft.Playwright;
using System;
using System.Text;
using Unbiased.Playwright.Application.Dto.PlaywrightDto;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrapping
{
    public class GetNewsWithGuidMethod
    {
        public async Task<List<News>> GetNewsWithGuid(List<SaveSearchUrlAndGuidDto> saveSearchUrlAndGuidDtos)
        {
            //todo for object reference error 
            try
            {
                var listOfNews = Enumerable.Empty<News>().ToList();
                using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
                var chromium = playwright.Chromium;
                var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = false,
                });

                var page = await browser.NewPageAsync();
                foreach (var news in saveSearchUrlAndGuidDtos.GroupBy(x => x.MatchId).ToList())
                {
                   
                    foreach (var url in news.ToList())
                    {
                        var stringBuilder = new StringBuilder();
                        await page.GotoAsync(url.Url, new PageGotoOptions
                        {
                            Timeout = 60000,
                            WaitUntil = WaitUntilState.DOMContentLoaded
                        });
                        var getAllNewContent = await page.QuerySelectorAllAsync("p");
                        foreach (var content in getAllNewContent)
                        {
                            stringBuilder.AppendLine(await content.InnerTextAsync());
                        }
                        listOfNews.Add(new News()
                        {
                            CreatedTime = DateTime.Now,
                            CreatedUser = "system",
                            Detail = stringBuilder.ToString(),
                            IsActive = true,
                            IsProcessed = false,
                            MatchId = url.MatchId,
                            IsDeleted = false,
                            Title = url.Title,
                            Url = url.Url,
                        });
                    }
                }
                await page.CloseAsync();
                return listOfNews;
            }
            catch (Exception exception)
            {

                throw exception.InnerException;
            }

        }
    }
}
