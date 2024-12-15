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
        Task<IEnumerable<GeneratedNews>> GetAllGeneratedNewsAsync();

        /// <summary>
        /// Retrieves all generated news with images asynchronously.
        /// </summary>
        /// <returns>A collection of generated news with images.</returns>
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync();
    }
}
