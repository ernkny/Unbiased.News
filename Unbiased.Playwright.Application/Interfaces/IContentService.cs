using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Application.Interfaces
{
    public interface IContentService
    {
        Task<bool> AddDailyHoroscopeAsync(HoroscopeDailyDetail horoscopeDetail);
    }
}
