using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Application.Interfaces
{
    /// <summary>
    /// Interface for engine service operations providing functionality for engine configuration and content generation.
    /// </summary>
    public interface IEngineService
    {
        /// <summary>
        /// Retrieves all engine configurations from the system.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing a collection of engine configuration DTOs.</returns>
        Task<IEnumerable<EngineConfigurationDto>> GetAllEngineConfigurationsAsync();

        /// <summary>
        /// Generates content based on the provided URL using the engine.
        /// </summary>
        /// <param name="url">The URL to generate content from.</param>
        /// <returns>A task that represents the asynchronous operation containing the generated content as a string.</returns>
        Task<string> GenerateContentAsync(string url);

        /// <summary>
        /// Deactivates or activates search functionality for the specified engine configuration.
        /// </summary>
        /// <param name="id">The unique identifier of the engine configuration to toggle.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> DeActivateOrActivateSearchAsync(string id);

        /// <summary>
        /// Activates the engine immediately for the specified configuration.
        /// </summary>
        /// <param name="id">The unique identifier of the engine configuration to activate immediately.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> ActivateEngineImmediatlyAsync(string id);
    }
}
