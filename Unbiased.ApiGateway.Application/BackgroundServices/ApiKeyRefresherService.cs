using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unbiased.ApiGateway.Application.Interfaces;
using Unbiased.ApiGateway.Common.Concrete.Helpers;

namespace Unbiased.ApiGateway.Application.BackgroundServices
{
    public class ApiKeyRefresherService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly string apiKeyPrefix = "Unbiased..281324!";

        public ApiKeyRefresherService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

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

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
