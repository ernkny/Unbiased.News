using Dapper;
using System.Data;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.DataAccess.Connections;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Concrete
{
    public class UserManagementRepository:IUserManagementRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManagementRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        public UserManagementRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

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

        public async Task<GetUserWithRolesDto> GetUserWithRolesAsync(int userId)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var userDictionary = new Dictionary<int, GetUserWithRolesDto>();

                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", userId);

                    var users = await connection.QueryAsync<GetUserWithRolesDto, Role, GetUserWithRolesDto>(
                        "UBFMW_sp_GetUserWithRolesById",
                        (user, role) =>
                        {
                            GetUserWithRolesDto userEntry;

                            if (!userDictionary.TryGetValue(user.UserId, out userEntry))
                            {
                                userEntry = user;
                                userEntry.Roles = new List<Role>();
                                userDictionary.Add(userEntry.UserId, userEntry);
                            }

                            userEntry.Roles.Add(role);
                            return userEntry;
                        },
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        splitOn: "RoleId"
                    );

                    return userDictionary.Values.FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

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
    }
}
