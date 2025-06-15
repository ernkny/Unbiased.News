using MediatR;
using Microsoft.Extensions.Configuration;
using Quartz;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Domain.Enums;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.Concrete.ExternalServices;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Application.Jobs
{
    /// <summary>
    /// Quartz job responsible for generating and storing daily horoscope data for all zodiac signs.
    /// This job is configured to not allow concurrent executions.
    /// </summary>
    [DisallowConcurrentExecution]
    public class GetDailyHoroscopeDataJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the GetDailyHoroscopeDataJob class.
        /// </summary>
        /// <param name="mediator">The mediator instance for handling commands and queries.</param>
        /// <param name="configuration">The application configuration.</param>
        public GetDailyHoroscopeDataJob(IMediator mediator, IConfiguration configuration, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _mediator = mediator;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Executes the job to generate daily horoscope content for each zodiac sign using an external API.
        /// Processes each horoscope type and stores the generated content in the database.
        /// </summary>
        /// <param name="context">The context in which the job is being executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                foreach (HoroscopeEnums horoscope in Enum.GetValues(typeof(HoroscopeEnums)))
                {

                    var GptApi = new GptApiExternalService(new HttpClient(), _configuration, _mediator, _serviceProvider, _eventAndActivityLog);
                    var horoscopedetail = await GptApi.SendHoroscopeToGptAndGetResponse(horoscope.ToString(), context.CancellationToken);
                    if (horoscopedetail != null)
                    {
                        var horoscopeData = new HoroscopeDailyDetail()
                        {
                            CreatedDate = DateTime.UtcNow,
                            HoroscopeId = (int)horoscope,
                            Detail = horoscopedetail
                        };

                        var result = await _mediator.Send(new InsertDailyHoroscopeCommand(horoscopeData));
                    }

                }
                await Task.CompletedTask;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }
    }
}
