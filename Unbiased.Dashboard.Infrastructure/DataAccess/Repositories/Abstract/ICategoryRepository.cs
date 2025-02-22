using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}
