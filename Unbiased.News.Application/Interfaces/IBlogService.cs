using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Application.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogWithImageDto>> GetAllBlogsWithImageAsync(string language,int pageNumber, string searchData);
        Task<int> GetAllBlogsWithImageCountAsync(string language, string? searchData);
        Task<BlogWithImageDto> GetBlogWithImageByUniqUrlAsync(string @UniqUrl);
    }
}
