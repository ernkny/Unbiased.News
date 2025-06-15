using Unbiased.Shared.Extensions.Concrete.Entities;

namespace Unbiased.Shared.Extensions.Concrete.Loggging
{
    /// <summary>
    ///  IEventAndActivityLog interface defines a contract for logging events and activities in the system.
    /// </summary>
    public interface IEventAndActivityLog
    {
        /// <summary>
        ///  Sends an event log message to a queue for processing.
        /// </summary>
        /// <param name="logMessage"></param>
        /// <returns></returns>
        Task<bool> SendEventLogToQueue(EventLog logMessage);
    }
}
