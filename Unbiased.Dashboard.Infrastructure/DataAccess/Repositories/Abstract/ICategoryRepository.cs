using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Represents a repository for managing customer contact messages in the Unbiased Dashboard.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Retrieves all categories from the database asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}
