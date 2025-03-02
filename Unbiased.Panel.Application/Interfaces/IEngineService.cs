using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Application.Interfaces
{
    public interface IEngineService
    {
        Task<IEnumerable<EngineConfigurationDto>> GetAllEngineConfigurationsAsync();
        Task<string> GenerateContentAsync(string url);

    }
}
