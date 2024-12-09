using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Unbiased.Shared.ExceptionHandler.Middleware.Concrete.Logs;
using Unbiased.Shared.ExceptionHandler.Middleware.Entities;

namespace Unbiased.Shared.ExceptionHandler.Middleware.Concrete.Middlewares
{
    /// <summary>
    /// Middleware for logging global activities.
    /// </summary>
    public class GlobalActivityLogMiddleware:AbstractEventAndActivityLog
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
                await SendActivityLogToQueue(context, _serviceProvider);
                await _next(context);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
