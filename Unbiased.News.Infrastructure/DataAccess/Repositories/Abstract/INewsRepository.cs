using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Defines the interface for a news repository.
    /// </summary>
    public interface INewsRepository
    {
        /// <summary>
        /// Retrieves all generated news asynchronously.
        /// </summary>
        /// <returns>A collection of generated news.</returns>
        Task<IEnumerable<GeneratedNew>> GetAllGeneratedNewsAsync(string language);

        /// <summary>
        /// Retrieves all generated news with images asynchronously.
        /// </summary>
        /// <returns>A collection of generated news with images.</returns>
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync(int categoryId, int pageNumber, string language);
        Task<int> GetAllGeneratedNewsWithImageCountAsync(int categoryId);

        Task<GenerateNewsWithImageDto> GetGeneratedNewsByUniqUrlWithImageAsync(string uniqUrlPath);
        Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdWithImageAsync(string id);
        Task<IEnumerable<GenerateNewsWithImageDto>> GetBannerGeneratedNewsWithImageAsync(int categoryId, string language)
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllLastTopGeneratedNewsWithCategoryIdForDetailAsync(int categoryId, string uniqUrlPath);
    }
}
