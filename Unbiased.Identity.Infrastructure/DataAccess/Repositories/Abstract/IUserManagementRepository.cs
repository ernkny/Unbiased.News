using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Interface for user management repository operations providing comprehensive CRUD functionality for user management, authentication, and role assignment.
    /// </summary>
    public interface IUserManagementRepository
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
        /// Validates if the provided username and email are available and not already in use.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        /// <param name="email">The email address to validate.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating availability.</returns>
        Task<bool> ValidateUsernameAndEmailWithRolesAsync(string username, string email);

        /// <summary>
        /// Retrieves a specific user with their assigned roles by user identifier.
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
        /// Checks if the provided email or username exists in the system and returns the user ID.
        /// </summary>
        /// <param name="EmailOrUsername">The email address or username to check.</param>
        /// <returns>A task that represents the asynchronous operation containing the user ID if found, or 0 if not found.</returns>
        Task<int> CheckEmailOrUsernameAsync(string EmailOrUsername);

        /// <summary>
        /// Retrieves the hashed password for a specific user by their identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation containing the hashed password.</returns>
        Task<string> GetHashedPasswordByIdAsync(int userId);

        /// <summary>
        /// Updates the refresh token and its expiration date for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="refreshToken">The new refresh token to store.</param>
        /// <param name="refreshTokenExpireDate">The expiration date for the refresh token.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> UpdateRefreshTokenByIdAsync(int userId, string refreshToken, DateTime? refreshTokenExpireDate);

        /// <summary>
        /// Retrieves the refresh token for a specific user by their identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation containing the refresh token.</returns>
        Task<string> GetRefreshTokenByIdAsync(int userId);

        /// <summary>
        /// Retrieves user information by their refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to search for.</param>
        /// <returns>A task that represents the asynchronous operation containing the user entity associated with the refresh token.</returns>
        Task<User> GetRefreshTokenWithTokenAsync(string refreshToken);
    }
}
