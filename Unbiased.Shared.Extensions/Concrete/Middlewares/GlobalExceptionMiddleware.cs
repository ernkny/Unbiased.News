using Microsoft.AspNetCore.Http;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Shared.Extensions.Concrete.Middlewares
{
    /// <summary>
    /// Global exception middleware to catch and handle exceptions in the application.
    /// </summary>
    public class GlobalExceptionMiddleware: AbstractEventAndActivityLog
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="serviceProvider">The service provider to resolve dependencies.</param>
        public GlobalExceptionMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Invokes the middleware and catches any exceptions that occur.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles an exception by logging it and returning a 500 error response.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="exception">The exception to handle.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var logMessage = new EventLog
            {
                EventType = $"{context.Request.Headers}",
                EventSeverity = "Error",
                Message = $"{exception.Message} ----- {exception.StackTrace} --- {exception.InnerException?.Message} --- {exception.InnerException?.StackTrace}",
                EventDate = DateTime.UtcNow
            };
            await SendEventLogToQueue(logMessage,_serviceProvider);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Internal Server Error");
        }

    }
}
