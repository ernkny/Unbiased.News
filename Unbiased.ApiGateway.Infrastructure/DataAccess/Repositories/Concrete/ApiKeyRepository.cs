using Dapper;
using Unbiased.ApiGateway.Infrastructure.DataAccess.Connections;
using Unbiased.ApiGateway.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.ApiGateway.Infrastructure.DataAccess.Repositories.Concrete
{
    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        public ApiKeyRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

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
