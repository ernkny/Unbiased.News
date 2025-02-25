using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface IBlogRepository
    {
        Task<IEnumerable<BlogWithImageDto>> GetAllBlogsWithImageAsync(int pageNumber, string searchData);
        Task<int> GetAllBlogsWithImageCountAsync(string? searchData);
        Task<BlogWithImageDto> GetBlogWithImageByUniqUrlAsync(string @UniqUrl);
    }
}
