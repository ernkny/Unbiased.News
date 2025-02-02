using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface IRoleManagementRepository
    {
        Task<IEnumerable<PagesWithPermissionsDto>> GetAllPagesWithPermissionsAsync();
        Task<IEnumerable<Role>> GetAllRolesAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Role>> GetAllRolessWithoutPaginationAsync();
        Task<int> GetAllRolesCountAsync();
        Task<bool> InsertRoleAsync(CreateRoleDto role);
        Task<RoleGetByIdDto> GetRoleById(int roleId);
        Task<bool> UpdateRoleAsync(UpdateRoleDto role);
        Task<int> DeleteRoleAsync(int id);


    }
}
