using MediatR;
using Microsoft.Extensions.Configuration;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.Concrete.ExternalServices;

namespace Unbiased.Playwright.Application.Services
{
    public sealed class NewsService : INewsService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public NewsService(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<Guid> AddNewsAsync(InsertNewsDto addNewsDto)
        {
            var result = await _mediator.Send(new AddNewsCommand(addNewsDto));
            return result;
        }

        public async Task<bool> SendNewsToApiForGenerateAsync()
        {
            var combindedNews = await _mediator.Send(new GetAllNewsCombinedDetailsQuery());
            var externalServiceSend=new GptApiExternalService(new HttpClient(), _configuration,_mediator);
            foreach (var item in combindedNews)
            {
                var result=await externalServiceSend.SendCombinedNewsDetailToGpt(item.CombinedDetails);
                if (result != null)
                {
                    var generatedNews = new News()
                    {
                        Detail = result.Detail,
                        Title = result.Title,
                        MatchId = item.MatchId
                    };
                    await _mediator.Send(new AddGeneratedNewsCommand(generatedNews));
                }
            }
            return true;
        }
    }
}
