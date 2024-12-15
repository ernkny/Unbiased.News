using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unbiased.ApiGateway.Application.Interfaces;
using Unbiased.ApiGateway.Common.Concrete.Helpers;

namespace Unbiased.ApiGateway.Application.BackgroundServices
{
    /// <summary>
    /// A hosted service responsible for periodically refreshing the API key.
    /// </summary>
    public class ApiKeyRefresherService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly string apiKeyPrefix = "Unbiased..281324!";

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeyRefresherService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider instance.</param>
        /// <param name="configuration">The configuration instance.</param>
        public ApiKeyRefresherService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        /// <summary>
        /// Starts the API key refresh service.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the start operation.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(RefreshApiKey, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
            
            return Task.CompletedTask;

        }

        private void RefreshApiKey(object state)
        {
            var newApiKey = ApiKeyGenerator.GenerateApiKeyWithPrefix(apiKeyPrefix);
            if (newApiKey is not null)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var apiKeyService = scope.ServiceProvider.GetRequiredService<IApiKeyService>();
                    apiKeyService.SetApiKeyAsync(newApiKey).Wait();
                    var getApiKey =  apiKeyService.GetApiKeyAsync().Result;
                    var filePath=_configuration.GetSection("FilePaths:ApiKeyPath").Value;
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        writer.WriteLine(getApiKey);
                    }
                }
            }
        }

        /// <summary>
        /// Stops the API key refresh service.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the stop operation.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Disposes the API key refresh service.
        /// </summary>
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
