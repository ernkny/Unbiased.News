using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete
{
    public class ContactRepository : IContactRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRepository
        /// <param name="connection">The Unbiased SQL connection.</param>
        public ContactRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Contact>> GetAllCustomerMessagesAsync(int pageNumber, int pageSize)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@pageNumber", pageNumber);
                    parameters.Add("@pageSize", pageSize);
                    return await connection.QueryAsync<Contact>("UB_sp_GetAllCustomerMessages", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Contact> GetCustomerMessagesByIdAsync(int id)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id);
                    return await connection.QueryFirstAsync<Contact>("UB_sp_GetCustomerMessagesById", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;

            }
        }


        public async Task<bool> DeleteCustomerMessagesByIdAsync(int id)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id);
                    return await connection.QueryFirstAsync<bool>("UB_sp_DeleteCustomerMessagesById", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;

            }
        }
    }
}
