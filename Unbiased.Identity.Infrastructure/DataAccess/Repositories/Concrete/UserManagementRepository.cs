using Dapper;
using System.Data;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.DataAccess.Connections;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Repository implementation for user management operations providing comprehensive CRUD functionality for user management, authentication, and role assignment with database access and error handling.
    /// </summary>
    public class UserManagementRepository:IUserManagementRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the UserManagementRepository class.
        /// </summary>
        /// <param name="connection">The SQL connection provider for database operations.</param>
        public UserManagementRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Retrieves all users with pagination support using stored procedure with error handling.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page for pagination.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of user entities.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during users retrieval.</exception>
        public async Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PageNumber", pageNumber);
                    parameters.Add("@PageSize", pageSize);
                    var result = await connection.QueryAsync<User>("UBFMW_sp_GetAllUsers", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the total count of all users in the system using stored procedure with error handling.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing the total count of users.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during users count retrieval.</exception>
        public async Task<int> GetAllUsersCountAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryFirstAsync<int>("UBFMW_sp_GetAllUsersCount", commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a new user with assigned roles in the system using stored procedure with error handling.
        /// </summary>
        /// <param name="user">The insert user with roles data transfer object containing user and role information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during user creation.</exception>
        public async Task<bool> InsertUserWithRolesAsync(InsertUserWithRolesDto user)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var roles = new DataTable();
                    roles.Columns.Add("RoleID", typeof(int));

                    foreach (var id in user.Roles)
                    {
                        roles.Rows.Add(id);
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@UserName", user.Username);
                    parameters.Add("@Password", user.Password);
                    parameters.Add("@Email", user.Email);
                    parameters.Add("@FirstName", user.FirstName);
                    parameters.Add("@Lastname", user.LastName);
                    parameters.Add("@Bio", user.Biography);
                    parameters.Add("@IsActive", user.IsActive);
                    parameters.Add("@Roles", roles.AsTableValuedParameter("ListOfRoles"));

                    return await connection.QueryFirstAsync<int>("UBFMW_sp_InsertUsersWithRoles", parameters, commandType: CommandType.StoredProcedure) == 1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates an existing user with new information and role assignments using stored procedure with error handling.
        /// </summary>
        /// <param name="user">The update user with roles data transfer object containing updated information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during user update.</exception>
        public async Task<bool> UpdateUserWithRolesAsync(UpdateUserWithRolesDto user)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var roles = new DataTable();
                    roles.Columns.Add("RoleID", typeof(int));

                    foreach (var id in user.Roles)
                    {
                        roles.Rows.Add(id);
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@userid", user.UserId);
                    parameters.Add("@FirstName", user.FirstName);
                    parameters.Add("@Lastname", user.LastName);
                    parameters.Add("@Bio", user.Biography);
                    parameters.Add("@IsActive", user.IsActive);
                    parameters.Add("@Roles", roles.AsTableValuedParameter("ListOfRoles"));

                    return await connection.QueryFirstAsync<int>("UBFMW_sp_UpdateUserWithRoles", parameters, commandType: CommandType.StoredProcedure) == 1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Validates if the provided username and email are available and not already in use using stored procedure with error handling.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        /// <param name="email">The email address to validate.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating availability.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during validation.</exception>
        public async Task<bool> ValidateUsernameAndEmailWithRolesAsync(string username, string email)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserName", username);
                    parameters.Add("@Email", email);

                    return await connection.QueryFirstAsync<int>("UBFMW_sp_ValidateUserData", parameters, commandType: CommandType.StoredProcedure) == 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Retrieves a specific user with their assigned roles and permissions by user identifier using stored procedure with error handling.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation containing the user with roles DTO.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during user retrieval.</exception>
        public async Task<GetUserWithRolesDto> GetUserWithRolesAsync(int userId)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var userDictionary = new Dictionary<int, GetUserWithRolesDto>();

                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", userId);

                    var users = await connection.QueryAsync<GetUserWithRolesDto, Role, Permission, GetUserWithRolesDto>(
                        "UBFMW_sp_GetUserWithRolesById",
                        (user, role, permission) =>
                        {
                            GetUserWithRolesDto userEntry;

                            if (!userDictionary.TryGetValue(user.UserId, out userEntry))
                            {
                                userEntry = user;
                                userEntry.Roles = new List<Role>();
                                userDictionary.Add(userEntry.UserId, userEntry);
                            }
                            Role roleEntry = userEntry.Roles.FirstOrDefault(r => r.RoleId == role.RoleId);
                            if (roleEntry == null)
                            {
                                roleEntry = role;
                                roleEntry.Permissions = new List<Permission>();
                                userEntry.Roles.Add(roleEntry);
                            }

                            roleEntry.Permissions.Add(permission);
                            return userEntry;
                        },
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        splitOn: "RoleId,PermissionId"
                    );

                    return userDictionary.Values.FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Deletes a user and their associated roles by user identifier using stored procedure with error handling.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during user deletion.</exception>
        public async Task<bool> DeleteUserWithRolesAsync(int userId)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@userid", userId);

                    return await connection.QueryFirstAsync<int>("UBFMW_sp_DeleteUserWithRoles", parameters, commandType: CommandType.StoredProcedure) == 1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Checks if the provided email or username exists in the system and returns the user ID using stored procedure with error handling.
        /// </summary>
        /// <param name="EmailOrUsername">The email address or username to check.</param>
        /// <returns>A task that represents the asynchronous operation containing the user ID if found, or 0 if not found.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during email or username check.</exception>
        public async Task<int> CheckEmailOrUsernameAsync(string EmailOrUsername)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmailOrUserName", EmailOrUsername);

                    return await connection.QueryFirstAsync<int>("UBFMW_sp_CheckEmailOrUserName", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Retrieves the hashed password for a specific user by their identifier using stored procedure with error handling.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation containing the hashed password.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during password retrieval.</exception>
        public async Task<string> GetHashedPasswordByIdAsync(int userId)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", userId);

                    return await connection.QueryFirstAsync<string>("UBFMW_sp_GetHashedPasswordById", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates the refresh token and its expiration date for a specific user using stored procedure with error handling.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="refreshToken">The new refresh token to store.</param>
        /// <param name="refreshTokenExpireDate">The expiration date for the refresh token.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during refresh token update.</exception>
        public async Task<bool> UpdateRefreshTokenByIdAsync(int userId,string refreshToken,DateTime? refreshTokenExpireDate)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", userId);
                    parameters.Add("@refreshToken", refreshToken);
                    parameters.Add("@refreshTokenExpiration", refreshTokenExpireDate, DbType.DateTime);
                    var result = await connection.QueryFirstOrDefaultAsync<int>(
                        "UBFMW_sp_UpdateRefreshTokenById",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    return result == 1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Retrieves the refresh token for a specific user by their identifier using stored procedure with error handling.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation containing the refresh token.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during refresh token retrieval.</exception>
        public async Task<string> GetRefreshTokenByIdAsync(int userId)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", userId);

                    return await connection.QueryFirstAsync<string>("UBFMW_sp_GetRefreshTokenIfExists", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Retrieves user information by their refresh token using stored procedure with error handling.
        /// </summary>
        /// <param name="refreshToken">The refresh token to search for.</param>
        /// <returns>A task that represents the asynchronous operation containing the user entity associated with the refresh token.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during user retrieval by refresh token.</exception>
        public async Task<User> GetRefreshTokenWithTokenAsync(string refreshToken)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@refreshToken", refreshToken);

                    return await connection.QueryFirstAsync<User>("UBFMW_sp_GetRefreshTokenWithTokenIfExists", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
