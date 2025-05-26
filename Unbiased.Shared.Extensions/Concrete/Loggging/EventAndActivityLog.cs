using Unbiased.Shared.Extensions.Concrete.Entities;

namespace Unbiased.Shared.Extensions.Concrete.Loggging
{
    /// <summary>
    ///  Represents a concrete implementation of event and activity logging.
    /// </summary>
    public class EventAndActivityLog:AbstractEventAndActivityLog
    {
        /// <summary>
        ///  Sends an event log to the queue.
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="_serviceProvider"></param>
        /// <returns></returns>
        public override async Task<bool> SendEventLogToQueue(EventLog logMessage, IServiceProvider _serviceProvider)
        {
            return await base.SendEventLogToQueue(logMessage, _serviceProvider);
        }
    }
}
