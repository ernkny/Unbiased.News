using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface IContentRepository
    {
        Task<bool> AddDailyHoroscopeAsync(HoroscopeDailyDetail horoscopeDetail);
    }
}
