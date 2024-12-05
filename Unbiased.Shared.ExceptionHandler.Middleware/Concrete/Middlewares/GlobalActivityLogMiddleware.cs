using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Unbiased.Log.Domain.Entities;

namespace Unbiased.Shared.ExceptionHandler.Middleware.Concrete.Middlewares
{
    /// <summary>
    /// Middleware for logging global activities.
    /// </summary>
    public class GlobalActivityLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalActivityLogMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next request delegate in the pipeline.</param>
        /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
        public GlobalActivityLogMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Invokes the middleware and sends the activity log to the queue.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await SendActivityLogToQueue(context);
                await _next(context);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sends the activity log to the queue.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if successful.</returns>
        private async Task<bool> SendActivityLogToQueue(HttpContext context)
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
                    ActivityDate = DateTime.UtcNow,
                    XForwardedFor = $"{context.Request.Headers["X-Forwarded-For"]}",
                    Referer = $"{context.Request.Headers["Referer"]}",
                    IP=context.Connection.RemoteIpAddress.ToString()
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
