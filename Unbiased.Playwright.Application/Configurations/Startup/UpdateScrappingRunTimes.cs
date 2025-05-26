using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;

namespace Unbiased.Playwright.Application.Configurations.Startup
{
    /// <summary>
    /// A hosted service that updates the next run time for scrapping search URLs.
    /// </summary>
    public class UpdateScrappingRunTimes : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UpdateScrappingRunTimes(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        ///  Starts the hosted service to update the next run time for scrapping search URLs.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var command = new UpdateSearchUrlNextRunTimeCommand();
                await mediator.Send(command);
            }
        }

        /// <summary>
        ///  Stops the hosted service gracefully.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
