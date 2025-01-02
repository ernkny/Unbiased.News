using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;

namespace Unbiased.Playwright.Application.Configurations.Startup
{
    public class UpdateScrappingRunTimes : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UpdateScrappingRunTimes(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var command = new UpdateSearchUrlNextRunTimeCommand();
                await mediator.Send(command);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
