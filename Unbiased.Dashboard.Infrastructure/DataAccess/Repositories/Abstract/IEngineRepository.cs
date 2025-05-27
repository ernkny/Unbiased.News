using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Represents a repository for managing news articles with images in the Unbiased Dashboard.
    /// </summary>
    public interface IEngineRepository
    {
        /// <summary>
        /// Retrieves all engine configurations for the dashboard.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EngineConfigurationDto>> GetAllEngineConfigurationsAsync();

        /// <summary>
        ///  Deactivates or activates a search engine based on the provided ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeActivateOrActivateSearchAsync(string id);

        /// <summary>
        /// Activates the engine immediately based on the provided ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ActivateEngineImmediatlyAsync(string id);
    }
}
