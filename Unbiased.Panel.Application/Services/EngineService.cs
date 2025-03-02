using MediatR;
using Microsoft.Extensions.Configuration;
using Unbiased.Dashboard.Application.Helpers.GptContentGenerator;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Engine;

namespace Unbiased.Dashboard.Application.Services
{
    public class EngineService : IEngineService
    {
        private readonly  IMediator _mediator;
        private readonly IConfiguration _configuration;

        public EngineService(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<string> GenerateContentAsync(string url)
        {
            try
            {
                
                var urlCanBeReached= await new HttpClient().GetAsync(url);
                if (!urlCanBeReached.IsSuccessStatusCode)
                {
                    throw new Exception("Url cannot be reached");
                }
                var cancelationToken= new CancellationTokenSource().Token;
                var generatedContent = await new ContentGenerator(_configuration).Generate(url, cancelationToken);
                return generatedContent;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<EngineConfigurationDto>> GetAllEngineConfigurationsAsync()
        {
            try
            {
               return await _mediator.Send(new GetAllEngineConfigurationsQuery());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
