using MediatR;
using Microsoft.Extensions.Configuration;
using Quartz;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Domain.Enums;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.Concrete.ExternalServices;

namespace Unbiased.Playwright.Application.Jobs
{
    public class GetDailyHoroscopeDataJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public GetDailyHoroscopeDataJob(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // todo datalar duplicate oluyor. yorumlar istenilen gibi değil prompt düzenle
                foreach (HoroscopeEnums horoscope in Enum.GetValues(typeof(HoroscopeEnums)))
                {
                    
                    var GptApi = new GptApiExternalService(new HttpClient(), _configuration, _mediator);
                    var horoscopedetail = await GptApi.SendHoroscopeToGptAndGetResponse(horoscope.ToString(), context.CancellationToken);
                    if (horoscopedetail != null)
                    {
                        var horoscopeData = new     HoroscopeDailyDetail()
                        {
                            CreatedDate = DateTime.UtcNow,
                            HoroscopeId = (int)horoscope, 
                            Detail = horoscopedetail
                        };

                        var result = await _mediator.Send(new InsertDailyHoroscopeCommand(horoscopeData));
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            
        }
    }
}
