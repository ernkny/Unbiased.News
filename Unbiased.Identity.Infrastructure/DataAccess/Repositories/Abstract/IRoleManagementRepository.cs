using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface IRoleManagementRepository
    {
        Task<IEnumerable<PagesWithPermissionsDto>> GetAllPagesWithPermissionsAsync();
    }
}
