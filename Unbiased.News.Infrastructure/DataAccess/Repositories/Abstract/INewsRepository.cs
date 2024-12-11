using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface INewsRepository
    {
        Task<IEnumerable<GeneratedNews>> GetAllGeneratedNewsAsync();
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync();
    }
}
