using MediatR;
using Unbiased.Log.Application.Interfaces;
using Unbiased.Shared.ExceptionHandler.Middleware.Entities;

namespace Unbiased.Log.Application.Services
{
    /// <summary>
    /// Provides logging services for inserting activity and event logs.
    /// </summary>
    public sealed class LogService : ILogService
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogService"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance for sending commands.</param>
        public LogService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Inserts an activity log asynchronously.
        /// </summary>
        /// <param name="activityLog">The activity log to insert.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<bool> InsertActivityLogAsync(ActivityLog activityLog)
        {
            // TODO: Implement the InsertActivityLogAsync method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts an event log asynchronously.
        /// </summary>
        /// <param name="eventLog">The event log to insert.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<bool> InsertEventLogAsync(EventLog eventLog)
        {
            // TODO: Implement the InsertEventLogAsync method
            throw new NotImplementedException();
        }
    }
}
