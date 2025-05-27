using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Application.Interfaces
{
    /// <summary>
    /// Interface for role management service operations providing comprehensive business logic for role and permission management.
    /// </summary>
    public interface IRoleManagementService
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
        Task<IEnumerable<Role>> GetAllRolesWithoutPaginationAsync();

        /// <summary>
        /// Gets the total count of all roles in the system.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing the total count of roles.</returns>
        Task<int> GetAllRolesCountAsync();

        /// <summary>
        /// Creates a new role with assigned permissions in the system.
        /// </summary>
        /// <param name="role">The create role data transfer object containing role and permission information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> CreateRoleAsync(CreateRoleDto role);

        /// <summary>
        /// Updates an existing role with new information and permission assignments.
        /// </summary>
        /// <param name="role">The update role data transfer object containing updated role and permission information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> UpdateRoleAsync(UpdateRoleDto role);

        /// <summary>
        /// Retrieves a specific role with its assigned permissions by role identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the role to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation containing the role DTO with detailed information and permissions.</returns>
        Task<RoleGetByIdDto> GetRoleByIdAsync(int id);

        /// <summary>
        /// Deletes a role by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the role to delete.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> DeleteRoleAsync(int id);
    }
}
