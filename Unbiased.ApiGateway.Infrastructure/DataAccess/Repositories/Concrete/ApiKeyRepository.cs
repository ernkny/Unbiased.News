using Dapper;
using Unbiased.ApiGateway.Infrastructure.DataAccess.Connections;
using Unbiased.ApiGateway.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.ApiGateway.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Repository for managing API keys.
    /// </summary>
    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeyRepository"/> class.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        public ApiKeyRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Retrieves the API key from the database.
        /// </summary>
        /// <returns>The API key.</returns>
        public async Task<string> GetApiKeyAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<string>("UB_sp_GetApiKey", commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Sets the API key in the database.
        /// </summary>
        /// <param name="apiKey">The API key to set.</param>
        /// <returns>True if the API key was set successfully; otherwise, false.</returns>
        public async Task<bool> SetApiKeyAsync(string apiKey)
        {

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("ApiKey", apiKey);
                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.ExecuteAsync("UB_sp_InsertApiKey", param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result == 1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
