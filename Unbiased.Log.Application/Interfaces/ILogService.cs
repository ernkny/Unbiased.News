using Unbiased.Log.Domain.Entities;

namespace Unbiased.Log.Application.Interfaces
{
    /// <summary>
    /// Defines the interface for log services.
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Inserts an event log asynchronously.
        /// </summary>
        /// <param name="eventLog">The event log to insert.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the insertion is successful.</returns>
        Task<bool> InsertEventLogAsync(EventLog eventLog);

        /// <summary>
        /// Inserts an activity log asynchronously.
        /// </summary>
        /// <param name="activityLog">The activity log to insert.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the insertion is successful.</returns>
        Task<bool> InsertActivityLogAsync(ActivityLog activityLog);
    }
}
