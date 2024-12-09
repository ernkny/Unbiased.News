using Unbiased.Shared.ExceptionHandler.Middleware.Entities;

namespace Unbiased.Log.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Defines the interface for a log repository, providing methods for inserting event and activity logs.
    /// </summary>
    public interface ILogRepository
    {
        /// <summary>
        /// Inserts an event log into the repository asynchronously.
        /// </summary>
        /// <param name="eventLog">The event log to be inserted.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the log was inserted successfully.</returns>
        Task<bool> InsertEventLogAsync(EventLog eventLog);

        /// <summary>
        /// Inserts an activity log into the repository asynchronously.
        /// </summary>
        /// <param name="activityLog">The activity log to be inserted.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the log was inserted successfully.</returns>
        Task<bool> InsertActivityLogAsync(ActivityLog activityLog);
    }
}
