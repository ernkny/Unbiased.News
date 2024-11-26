using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Application.Interfaces.Playwright
{
    public interface IPlaywrightScrappingService
    {
        Task<List<News>> PlaywrightScrappingNewsAsync();
        Task<bool> SaveAllNewsWithRangeAsync(List<News> listOfNews);
        Task<bool> PlaywrightScrappingNewsAndAddRangeNewsAsync();
        Task<List<News>> PlaywrightScrappingNews();
        Task<bool> SaveAllNewsWithRange(List<News> listOfNews);
        Task<bool> PlaywrightScrappingNewsAndAddRangeNews();
        Task<bool> GetImagesForCollectedNews();
    }
}
