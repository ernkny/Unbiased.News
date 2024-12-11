using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Application.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<GeneratedNews>> GetAllGeneratedNewsAsync();
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync();
    }
}
