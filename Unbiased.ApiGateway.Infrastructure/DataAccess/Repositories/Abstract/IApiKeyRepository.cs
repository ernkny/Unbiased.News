namespace Unbiased.ApiGateway.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface IApiKeyRepository
    {
        Task<string> GetApiKeyAsync();
        Task<bool> SetApiKeyAsync(string apiKey);
    }
}
