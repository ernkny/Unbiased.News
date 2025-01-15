using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface IContentRepository
    {
        Task<Contents> GetLastContentAsync();
        Task<IEnumerable<HoroscopeDailyDetail>> GetDailyLastHoroscopesAsync();
    }
}
