using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Interface for managing categories.
    /// </summary>
    public interface ICategoriesRepository
    {
        /// <summary>
        /// Retrieves all categories from the repository asynchronously.
        /// </summary>
        /// <returns>An enumerable collection of Category objects representing all categories.</returns>
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<IEnumerable<HomePageCategorieSliderWithCountDto>> GetHomePageCategorieSliderWithCountAsync();
        Task<IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>> GetHomePageCategoriesRandomLastGeneratedNewsAsync();
        Task<IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>> GetHomePageTopCategoriesGeneratedNewsAsync();
    }
}
