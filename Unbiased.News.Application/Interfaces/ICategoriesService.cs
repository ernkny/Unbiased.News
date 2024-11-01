using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Application.Interfaces
{
    public interface ICategoriesService
    {
        Task<List<Category>> GetAllCategoriesAsync();
    }
}
