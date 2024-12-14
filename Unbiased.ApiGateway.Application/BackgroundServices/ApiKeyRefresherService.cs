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

        public ApiKeyRefresherService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private readonly string apiKeyPrefix = "Unbiased..281324!";

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
                    apiKeyService.SetApiKeyAsync(newApiKey);
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
