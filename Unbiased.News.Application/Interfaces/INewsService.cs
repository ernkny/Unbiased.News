using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Application.Interfaces
{
    /// <summary>
    /// Defines the interface for news services.
    /// </summary>
    public interface INewsService
    {
        /// <summary>
        /// Retrieves all generated news asynchronously.
        /// </summary>
        /// <returns>A task containing a collection of generated news.</returns>
        Task<IEnumerable<GeneratedNew>> GetAllGeneratedNewsAsync(string language);

        /// <summary>
        /// Retrieves all generated news with images asynchronously.
        /// </summary>
        /// <returns>A task containing a collection of generated news with images.</returns>
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync(int categoryId,int pageNumber, string language);
        Task<int> GetAllGeneratedNewsWithImageCountAsync(int categoryId);

        Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdAsync(string id);
        Task<GenerateNewsWithImageDto> GetGeneratedNewsByUniqUrlAsync(string UniqUrl);

        Task<IEnumerable<GenerateNewsWithImageDto>> GetBannerGeneratedNewsWithImageAsync();
    }
}
