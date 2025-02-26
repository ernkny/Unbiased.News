using Dapper;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete
{
    public class EngineRepository:IEngineRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public EngineRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<EngineConfigurationDto>> GetAllEngineConfigurationsAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryAsync<EngineConfigurationDto>("UB_sp_GetAllEngineConfigurationsForDashboard");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
