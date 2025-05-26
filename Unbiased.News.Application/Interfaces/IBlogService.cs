using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Application.Interfaces
{
    /// <summary>
    /// Interface for blog-related services.
    /// </summary>
    public interface IBlogService
    {
        /// <summary>
        ///  Retrieves all blogs with images asynchronously.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="pageNumber"></param>
        /// <param name="searchData"></param>
        /// <returns></returns>
        Task<IEnumerable<BlogWithImageDto>> GetAllBlogsWithImageAsync(string language,int pageNumber, string searchData);

        /// <summary>
        /// Retrieves the count of all blogs with images asynchronously.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="searchData"></param>
        /// <returns></returns>
        Task<int> GetAllBlogsWithImageCountAsync(string language, string? searchData);

        /// <summary>
        /// Retrieves a blog with image by its unique URL asynchronously.
        /// </summary>
        /// <param name="UniqUrl"></param>
        /// <returns></returns>
        Task<BlogWithImageDto> GetBlogWithImageByUniqUrlAsync(string @UniqUrl);
    }
}
