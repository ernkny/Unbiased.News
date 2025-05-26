using Unbiased.News.Domain.DTOs;
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

        /// <summary>
        ///  Retrieves categories for the home page slider with count asynchronously.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        Task<List<HomePageCategorieSliderWithCountDto>> GetHomePageCategorieSliderWithCountAsync(string language);

        /// <summary>
        /// Retrieves categories for the home page with random last generated news asynchronously.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        Task<List<HomePageCategoriesRandomLastGeneratedNewsDto>> GetHomePageCategoriesRandomGeneratedNewsAsync(string language);

        /// <summary>
        /// Retrieves the top categories with last generated news for the home page asynchronously.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        Task<List<HomePageCategoriesRandomLastGeneratedNewsDto>> GetHomePageTopCategoriesGeneratedNewsAsync(string language);
    }
}
