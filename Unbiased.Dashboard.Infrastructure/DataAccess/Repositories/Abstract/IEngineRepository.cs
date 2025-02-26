using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface IEngineRepository
    {
        Task<IEnumerable<EngineConfigurationDto>> GetAllEngineConfigurationsAsync();
    }
}
