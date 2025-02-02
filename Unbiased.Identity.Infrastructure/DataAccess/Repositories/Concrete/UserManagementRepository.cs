using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
