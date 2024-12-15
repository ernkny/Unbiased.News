namespace Unbiased.ApiGateway.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Defines the interface for an API key repository.
    /// </summary>
    public interface IApiKeyRepository
    {
        /// <summary>
        /// Retrieves the API key asynchronously.
        /// </summary>
        /// <returns>The API key as a string.</returns>
        Task<string> GetApiKeyAsync();

        /// <summary>
        /// Sets the API key asynchronously.
        /// </summary>
        /// <param name="apiKey">The API key to set.</param>
        /// <returns>True if the API key was set successfully; otherwise, false.</returns>
        Task<bool> SetApiKeyAsync(string apiKey);
    }
}
