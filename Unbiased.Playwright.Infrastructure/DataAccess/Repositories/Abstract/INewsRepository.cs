using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface INewsRepository
    {
        Task<Guid> AddNewsAsync(InsertNewsDto addNewsDto);
        Task<IEnumerable<News>> GetAllNewsByNotIncludedProcessAsync();
        Task<IEnumerable<string>> GetAllActiveKeywordsForSearchAsync();
        Task<bool> AddRangeAllNewsAsync(IEnumerable<News> news);
        Task<bool> ValidateUrlForSearchWithTitleAsync(string title);
        Task<IEnumerable<GeneratedNewsDto>> GetAllCombinedDetailsAsync();
        Task<bool> AddOpenAiResponseAsync(string responseBody);
        Task<bool> AddGeneratedNews(News generatedNews);
        Task<bool> UpdateNewsProcessValueAsTrueAsync(string matchId);
    }
}
