using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Dto_s.Login;
using Unbiased.Identity.Domain.Dtos.Authentication;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Application.Interfaces
{
    /// <summary>
    /// Interface for user management service operations providing comprehensive business logic for user management, authentication, and role assignment.
    /// </summary>
    public interface IUserManagementService
    {
        /// <summary>
        /// Retrieves all users with pagination support.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page for pagination.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of user entities.</returns>
        Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Gets the total count of all users in the system.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing the total count of users.</returns>
        Task<int> GetAllUsersCountAsync();

        /// <summary>
        /// Creates a new user with assigned roles in the system.
        /// </summary>
        /// <param name="user">The insert user with roles data transfer object containing user and role information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> InsertUserWithRolesAsync(InsertUserWithRolesDto user);

        /// <summary>
        /// Retrieves a specific user with their assigned roles and permissions by user identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation containing the user with roles DTO.</returns>
        Task<GetUserWithRolesDto> GetUserWithRolesAsync(int userId);

        /// <summary>
        /// Updates an existing user with new information and role assignments.
        /// </summary>
        /// <param name="user">The update user with roles data transfer object containing updated information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> UpdateUserWithRolesAsync(UpdateUserWithRolesDto user);

        /// <summary>
        /// Deletes a user and their associated roles by user identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> DeleteUserWithRolesAsync(int userId);

        /// <summary>
        /// Authenticates a user with their credentials and returns a JWT token.
        /// </summary>
        /// <param name="loginDto">The login data transfer object containing user credentials.</param>
        /// <returns>A task that represents the asynchronous operation containing the authentication token DTO.</returns>
        Task<TokenDto> LoginAsync(LoginDto loginDto);
    }
}
