using Unbiased.ApiGateway.Application.Interfaces;
using Unbiased.ApiGateway.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.ApiGateway.Application.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IApiKeyRepository _apiKeyRepository;

        public ApiKeyService(IApiKeyRepository apiKeyRepository)
        {
            _apiKeyRepository = apiKeyRepository;
        }

        public async Task<string> GetApiKeyAsync()
        {

            return await _apiKeyRepository.GetApiKeyAsync();


        }

        public async Task<bool> SetApiKeyAsync(string apiKey)
        {

            return await _apiKeyRepository.SetApiKeyAsync(apiKey);

        }
    }
}
