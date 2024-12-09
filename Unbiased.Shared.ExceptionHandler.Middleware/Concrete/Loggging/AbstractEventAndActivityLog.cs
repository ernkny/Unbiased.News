using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using Unbiased.Shared.ExceptionHandler.Middleware.Entities;

namespace Unbiased.Shared.ExceptionHandler.Middleware.Concrete.Logs
{
    public abstract class AbstractEventAndActivityLog
    {

        /// <summary>
        /// Sends an event log to the queue.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="logMessage">The log message to send.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<bool> SendEventLogToQueue(EventLog logMessage,IServiceProvider _serviceProvider)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var sendEndpointProvider = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();

                var timeout = TimeSpan.FromSeconds(30);
                using var source = new CancellationTokenSource(timeout);
                var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:EventLogMessageQueue"));
                if (sendEndpoint == null)
                {
                    throw new Exception("SendEndpoint is null");
                }
                await sendEndpoint.Send(logMessage, source.Token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sends the activity log to the queue.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if successful.</returns>
        public async Task<bool> SendActivityLogToQueue(HttpContext context, IServiceProvider _serviceProvider)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var sendEndpointProvider = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();
                var activityLog = new ActivityLog()
                {
                    ActionType = context.Request.Method,
                    Endpoint = context.Request.Path,
                    Message = $"Info for {context.Request.Headers["Accept"]} performed action {context.Request.Method} on endpoint {context.Request.Path}",
                    ActivityDate = DateTime.Now,
                    XForwardedFor = $"{context.Request.Headers["X-Forwarded-For"]}",
                    Referer = $"{context.Request.Headers["Referer"]}",
                    IP = context.Connection.RemoteIpAddress.ToString()
                };
                var timeout = TimeSpan.FromSeconds(30);
                using var source = new CancellationTokenSource(timeout);
                var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:ActivityLogMessageQueue"));
                if (sendEndpoint == null)
                {
                    throw new Exception("SendEndpoint is null");
                }
                await sendEndpoint.Send(activityLog, source.Token);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
