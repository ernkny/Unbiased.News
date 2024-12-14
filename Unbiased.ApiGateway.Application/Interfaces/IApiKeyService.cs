namespace Unbiased.ApiGateway.Application.Interfaces
{
    public interface IApiKeyService
    {
        Task<string> GetApiKeyAsync();
        Task<bool> SetApiKeyAsync(string apiKey);
    }
}
