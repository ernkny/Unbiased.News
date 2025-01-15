using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Application.Interfaces
{
    public interface IContentService
    {

        Task<Contents> GetLastContentAsync();
        Task<IEnumerable<HoroscopeDailyDetail>> GetDailyLastHoroscopesAsync();
    }
}
