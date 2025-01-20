using MediatR;
using Microsoft.Extensions.Configuration;
using Quartz;
using Unbiased.Playwright.Application.Exceptions.Custom;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;

namespace Unbiased.Playwright.Application.Jobs
{
    [DisallowConcurrentExecution]
    public class ConsumeUnprocessedImagesJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly INewsService _newsService;

        public ConsumeUnprocessedImagesJob(IMediator mediator, INewsService newsService, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _newsService = newsService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var images = await _mediator.Send(new GetImagesWithNoneHasGeneratedQuery());
                if (images.Any())
                {
                    foreach (var image in images) {

                        await _newsService.GenerateImagesWhenAllNewsHasGeneratedAsync(context.CancellationToken);
                    }

                }
                await Task.CompletedTask;
            }
            catch (Exception ex) when (ex.Message.Contains("TooManyRequests"))
            {
                throw new TooManyRequestsException("API returned error: TooManyRequests", ex);
            }
            catch (TooManyRequestsException exception)
            {
                Console.WriteLine(exception.Message);
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
