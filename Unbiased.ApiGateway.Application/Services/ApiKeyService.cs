using Unbiased.ApiGateway.Application.Interfaces;
using Unbiased.ApiGateway.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.ApiGateway.Application.Services
{
    /// <summary>
    /// Provides a service for managing API keys.
    /// </summary>
    public class ApiKeyService : IApiKeyService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeyService"/> class.
        /// </summary>
        /// <param name="apiKeyRepository">The API key repository instance.</param>
        public ApiKeyService(IApiKeyRepository apiKeyRepository)
        {
            _apiKeyRepository = apiKeyRepository;
        }

        private readonly IApiKeyRepository _apiKeyRepository;

        /// <summary>
        /// Retrieves the current API key asynchronously.
        /// </summary>
        /// <returns>A task representing the API key retrieval operation.</returns>
        public async Task<string> GetApiKeyAsync()
        {
            return await _apiKeyRepository.GetApiKeyAsync();
        }

        /// <summary>
        /// Sets the API key asynchronously.
        /// </summary>
        /// <param name="apiKey">The new API key to set.</param>
        /// <returns>A task representing the API key setting operation.</returns>
        public async Task<bool> SetApiKeyAsync(string apiKey)
        {
            return await _apiKeyRepository.SetApiKeyAsync(apiKey);
        }
    }
}
