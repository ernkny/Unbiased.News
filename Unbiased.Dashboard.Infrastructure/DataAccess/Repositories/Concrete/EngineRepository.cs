using Dapper;
using System.Data;
using System.Runtime.CompilerServices;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Represents a repository for managing engine configurations in the Unbiased Dashboard.
    /// </summary>
    public class EngineRepository : IEngineRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        /// <param name="eventAndActivityLog">The event and activity logging service.</param>
        public EngineRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Retrieves all engine configurations for the dashboard.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the collection of engine configuration DTOs.</returns>
        public async Task<IEnumerable<EngineConfigurationDto>> GetAllEngineConfigurationsAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryAsync<EngineConfigurationDto>("UB_sp_GetAllEngineConfigurationsForDashboard");
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Deactivates or activates a search engine based on the provided ID.
        /// </summary>
        /// <param name="id">The unique identifier of the search engine to activate or deactivate.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the operation was successful; otherwise, false.</returns>
        public async Task<bool> DeActivateOrActivateSearchAsync(string id)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("id", id);
                    return await connection.QueryFirstAsync<bool>("UB_sp_DeActivateOrActivateSearch", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Activates an engine immediately based on the provided ID.
        /// </summary>
        /// <param name="id">The unique identifier of the engine to activate immediately.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the activation was successful; otherwise, false.</returns>
        public async Task<bool> ActivateEngineImmediatlyAsync(string id)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("id", id);
                    return await connection.QueryFirstAsync<bool>("UB_sp_ActivateEngineImmediatly", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }
    }
}
