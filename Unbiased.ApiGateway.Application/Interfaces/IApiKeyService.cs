namespace Unbiased.ApiGateway.Application.Interfaces
{
    /// <summary>
    /// Represents a service for managing API keys.
    /// </summary>
    public interface IApiKeyService
    {
        /// <summary>
        /// Retrieves the current API key asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the API key as a string.</returns>
        Task<string> GetApiKeyAsync();

        /// <summary>
        /// Sets the API key asynchronously.
        /// </summary>
        /// <param name="apiKey">The API key to set.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the operation was successful.</returns>
        Task<bool> SetApiKeyAsync(string apiKey);
    }
}
