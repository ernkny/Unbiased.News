using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Application.Interfaces
{
    /// <summary>
    /// Defines the interface for categories service.
    /// </summary>
    public interface ICategoriesService
    {
        /// <summary>
        /// Retrieves a list of all categories asynchronously.
        /// </summary>
        /// <returns>A list of categories.</returns>
        Task<List<Category>> GetAllCategoriesAsync();
    }
}
