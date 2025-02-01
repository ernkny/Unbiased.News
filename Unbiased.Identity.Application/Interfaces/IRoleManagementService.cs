using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Application.Interfaces
{
    public interface IRoleManagementService
    {
        Task<IEnumerable<PagesWithPermissionsDto>> GetAllPagesWithPermissionsAsync();
        Task<IEnumerable<Role>> GetAllRolesAsync(int pageNumber, int pageSize);
        Task<int> GetAllRolesCountAsync();
        Task<bool> CreateRoleAsync(CreateRoleDto role);
        Task<bool> UpdateRoleAsync(UpdateRoleDto role);
        Task<RoleGetByIdDto> GetRoleByIdAsync(int id);

        Task<bool> DeleteRoleAsync(int id);

    }
}
