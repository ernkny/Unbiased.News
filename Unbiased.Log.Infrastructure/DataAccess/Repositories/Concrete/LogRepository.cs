using Dapper;
using Unbiased.Log.Domain.Entities;
using Unbiased.Log.Infrastructure.DataAccess.Connections;
using Unbiased.Log.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Log.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Represents a repository for logging operations.
    /// </summary>
    public class LogRepository:ILogRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Represents a repository for logging operations.
        /// </summary>
        public LogRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Inserts an event log into the database asynchronously.
        /// </summary>
        /// <param name="eventLog">The event log to insert.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the log was inserted successfully.</returns>
        public async Task<bool> InsertEventLogAsync(EventLog eventLog)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@@EventType", eventLog.EventType);
                    dynamicParameters.Add("@EventSeverity", eventLog.EventSeverity);
                    dynamicParameters.Add("@EventDate", eventLog.EventDate);
                    dynamicParameters.Add("@Message", eventLog.Message);
                    var result = await connection.ExecuteAsync("UB_sp_InsertEventLog", dynamicParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result == 1;
                }
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Inserts an activity log into the database asynchronously.
        /// </summary>
        /// <param name="activityLog">The activity log to insert.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the log was inserted successfully.</returns>
        public async Task<bool> InsertActivityLogAsync(ActivityLog activityLog)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ActionType", activityLog.ActionType);
                    dynamicParameters.Add("@UserId", activityLog.UserId);
                    dynamicParameters.Add("@Message", activityLog.Message);
                    dynamicParameters.Add("@ActivityDate", activityLog.ActivityDate);
                    dynamicParameters.Add("@IP", activityLog.IP);
                    dynamicParameters.Add("@XForwardedFor", activityLog.XForwardedFor);
                    dynamicParameters.Add("@referer", activityLog.Referer);
                    dynamicParameters.Add("@Endpoint", activityLog.Endpoint);
                    var result = await connection.ExecuteAsync("UB_sp_InsertActivityLog", dynamicParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result == 1;
                }
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

        }
    }
}
