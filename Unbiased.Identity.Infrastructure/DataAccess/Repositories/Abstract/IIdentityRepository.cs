using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Interface for identity repository operations providing functionality for role and permission management.
    /// </summary>
    public interface IIdentityRepository
    {
        /// <summary>
        /// Inserts a new role with its associated permissions into the system.
        /// </summary>
        /// <param name="roleWithPermissionDto">The role with permission data transfer object containing role and permission information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> InsertRoleWithPermissions(RoleWithPermissionDto roleWithPermissionDto);
    }
}
