using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Application.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogWithImageDto>> GetAllBlogsWithImageAsync(int pageNumber, string searchData);
        Task<int> GetAllBlogsWithImageCountAsync(string? searchData);
        Task<BlogWithImageDto> GetBlogWithImageByUniqUrlAsync(string @UniqUrl);
    }
}
