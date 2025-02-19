using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Unbiased.Shared.Extensions.Concrete.Middlewares
{
    public class ApiKeyAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public ApiKeyAuthorizeMiddleware(RequestDelegate next, IServiceProvider serviceProvider,IConfiguration configuration)
        {
            _next = next;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var checkApiKey = context.Request.Headers["X-Api-Key"];
            var apiKeyPath= _configuration.GetSection("FilePaths:ApiKeyPath").Value;
            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key was not provided");
                return;
            }

            if (string.IsNullOrEmpty(checkApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }
            var apiKey = string.Empty;
            using (StreamReader r = new StreamReader(apiKeyPath))
            {
                apiKey = r.ReadToEnd().Trim();
            }
            if (string.IsNullOrEmpty(apiKey))
            {
                using (FileStream fileStream = new FileStream(apiKeyPath, FileMode.Create))
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine("DefaultApiKeyForEmptyLineSolution");
                }
            }

            if (apiKey != checkApiKey)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }
            await _next(context);
        }
    }
}
