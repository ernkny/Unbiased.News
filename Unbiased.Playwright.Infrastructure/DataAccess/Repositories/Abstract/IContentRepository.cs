using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Interface for content repository that provides methods for managing content-related data.
    /// </summary>
    public interface IContentRepository
    {
        /// <summary>
        /// Retrieves the most recent content entry from the database.
        /// </summary>
        /// <param name="horoscopeDetail"></param>
        /// <returns></returns>
        Task<bool> AddDailyHoroscopeAsync(HoroscopeDailyDetail horoscopeDetail);

        /// <summary>
        /// Retrieves the most recent content entry from the database.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<bool> AddDailyContentInformationAsync(Contents content);

        /// <summary>
        /// Retrieves the most recent content entry from the database.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<ContentSubHeading>> GetAllNoneGeneratedSubHeadingsAsync(CancellationToken token);

        /// <summary>
        /// Retrieves the most recent content entry from the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> AddGeneratedContentAsync(InsertAllContentDataRequest request);

        /// <summary>
        /// Retrieves the most recent content entry from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<bool> InsertContentSubheadings(int id, string title);

        /// <summary>
        /// Retrieves the most recent content entry from the database.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ContentCategory>> GetAllContentCategories();
    }
}
