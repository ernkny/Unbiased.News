using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Initializes a new instance of the <see cref="CategoriesRepository"/> class.
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

                return await connection.QueryFirstAsync<int>("UBFMW_InsertUsersWithRoles", parameters, commandType: CommandType.StoredProcedure)==1;
            }

        }

        public async Task<bool> ValidateUserWithRolesAsync(InsertUserWithRolesDto user)
        {
            using (var connection = _connection.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", user.Username);
                parameters.Add("@Email", user.Email);

                return await connection.QueryFirstAsync<int>("UBFMW_sp_ValidateUserData", parameters, commandType: CommandType.StoredProcedure) == 0;
            }

        }
    }
}
