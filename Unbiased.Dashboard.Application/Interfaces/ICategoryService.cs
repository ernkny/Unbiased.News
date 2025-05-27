using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Application.Interfaces
{
    /// <summary>
    /// Interface for category service operations providing functionality for category management.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Retrieves all categories from the system.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing a collection of category entities.</returns>
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}
