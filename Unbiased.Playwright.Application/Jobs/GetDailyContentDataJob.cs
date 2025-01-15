using MediatR;
using Microsoft.Extensions.Configuration;
using Quartz;
using System.Runtime.CompilerServices;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.Concrete.ExternalServices;

namespace Unbiased.Playwright.Application.Jobs
{
    public class GetDailyContentDataJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public GetDailyContentDataJob(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {

                if (context != null)
                {
                    var dailyContentDataFromGpt = new GptApiExternalService(new HttpClient(), _configuration, _mediator);
                    var content = await dailyContentDataFromGpt.SendDailyInformationToGptAndGetResponse(context.CancellationToken);
                    var contentDetail = new Contents()
                    {
                        ContentCategoryId = 1,
                        ContentDetail = content,
                        CreatedDate = DateTime.Now,
                    };
                    await _mediator.Send(new InsertDailyContentCommand(contentDetail), context.CancellationToken);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
