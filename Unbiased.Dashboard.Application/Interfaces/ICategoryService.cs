using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}
