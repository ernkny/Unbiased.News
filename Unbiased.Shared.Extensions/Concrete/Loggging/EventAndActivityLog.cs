using MassTransit;
using Microsoft.AspNetCore.Http;
using Unbiased.Shared.Extensions.Concrete.Entities;

namespace Unbiased.Shared.Extensions.Concrete.Loggging
{
    /// <summary>
    ///  Represents a concrete implementation of event and activity logging.
    /// </summary>
    public class EventAndActivityLog : IEventAndActivityLog
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public EventAndActivityLog(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        /// <summary>
        ///  Sends an event log to the queue.
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="_serviceProvider"></param>
        /// <returns></returns>
        public async Task<bool> SendEventLogToQueue(EventLog logMessage)
        {
            try
            {
                using var source = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:EventLogMessageQueue"));
                await endpoint.Send(logMessage, source.Token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///  Sends an activity log to the queue based on the provided HTTP context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> SendActivityLogToQueue(HttpContext context)
        {
            try
            {
                var activityLog = new ActivityLog
                {
                    ActionType = context.Request.Method,
                    Endpoint = context.Request.Path,
                    Message = $"Info for {context.Request.Headers["Accept"]} performed action {context.Request.Method} on endpoint {context.Request.Path}",
                    ActivityDate = DateTime.UtcNow,
                    XForwardedFor = context.Request.Headers["X-Forwarded-For"],
                    Referer = context.Request.Headers["Referer"],
                    IP = context.Connection.RemoteIpAddress?.ToString()
                };

                using var source = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:ActivityLogMessageQueue"));

                if (sendEndpoint == null)
                    throw new Exception("SendEndpoint is null");

                await sendEndpoint.Send(activityLog, source.Token);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
