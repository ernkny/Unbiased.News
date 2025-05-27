using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Interface for role management repository operations providing comprehensive CRUD functionality for role and permission management.
    /// </summary>
    public interface IRoleManagementRepository
    {
        /// <summary>
        /// Retrieves all pages with their associated permissions from the system.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing a collection of pages with permissions DTOs.</returns>
        Task<IEnumerable<PagesWithPermissionsDto>> GetAllPagesWithPermissionsAsync();

        /// <summary>
        /// Retrieves all roles with pagination support.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page for pagination.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of role entities.</returns>
        Task<IEnumerable<Role>> GetAllRolesAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves all roles without pagination.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing a collection of all role entities.</returns>
        Task<IEnumerable<Role>> GetAllRolessWithoutPaginationAsync();

        /// <summary>
        /// Gets the total count of all roles in the system.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing the total count of roles.</returns>
        Task<int> GetAllRolesCountAsync();

        /// <summary>
        /// Creates a new role in the system.
        /// </summary>
        /// <param name="role">The create role data transfer object containing role information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> InsertRoleAsync(CreateRoleDto role);

        /// <summary>
        /// Retrieves a specific role by its unique identifier.
        /// </summary>
        /// <param name="roleId">The unique identifier of the role to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation containing the role DTO with detailed information.</returns>
        Task<RoleGetByIdDto> GetRoleById(int roleId);

        /// <summary>
        /// Updates an existing role with new information.
        /// </summary>
        /// <param name="role">The update role data transfer object containing updated role information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> UpdateRoleAsync(UpdateRoleDto role);

        /// <summary>
        /// Deletes a role by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the role to delete.</param>
        /// <returns>A task that represents the asynchronous operation containing the number of affected rows.</returns>
        Task<int> DeleteRoleAsync(int id);
    }
}
