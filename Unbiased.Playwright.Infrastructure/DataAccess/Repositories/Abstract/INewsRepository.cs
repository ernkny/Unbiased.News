using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface INewsRepository
    {
        Task<Guid> AddNewsAsync(InsertNewsDto addNewsDto);
        Task<IEnumerable<News>> GetAllNewsByNotIncludedProcessAsync();
        Task<IEnumerable<string>> GetAllActiveKeywordsForSearchAsync();

        Task<bool> AddRangeAllNews(IEnumerable<News> news);
    }
}
