using MediatR;
using Microsoft.Extensions.Configuration;
using Quartz;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.Concrete.ExternalServices;

namespace Unbiased.Playwright.Application.Jobs
{
    public class ConsumeUnprocessedNewsJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly INewsService _newsService;
        private readonly IConfiguration _configuration;

        public ConsumeUnprocessedNewsJob(IMediator mediator, INewsService newsService, IConfiguration configuration)
        {
            _mediator = mediator;
            _newsService = newsService;
            _configuration = configuration;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var checkNews = await _mediator.Send(new GetAllNewsByNotIncludedProcessQuery());
                if (checkNews.Any())
                {
                    var combinedNews = await _mediator.Send(new GetAllNewsCombinedDetailsQuery(), context.CancellationToken);
                    var externalServiceSend = new GptApiExternalService(new HttpClient(), _configuration, _mediator);
                    await _newsService.GenerateNewsWithApiAsync(combinedNews, context.CancellationToken, externalServiceSend);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
