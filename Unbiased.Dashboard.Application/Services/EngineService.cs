using MediatR;
using Microsoft.Extensions.Configuration;
using Unbiased.Dashboard.Application.Helpers.GptContentGenerator;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Engine;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Engine;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Application.Services
{
    /// <summary>
    /// Service implementation for engine operations providing functionality for engine configuration, content generation, and search management with error handling and logging.
    /// </summary>
    public sealed class EngineService : IEngineService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the EngineService class.
        /// </summary>
        /// <param name="mediator">The mediator for handling CQRS operations.</param>
        /// <param name="configuration">The configuration provider for application settings.</param>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        public EngineService(IMediator mediator, IConfiguration configuration, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _mediator = mediator;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Deactivates or activates search functionality for the specified engine configuration with error handling and logging.
        /// </summary>
        /// <param name="id">The unique identifier of the engine configuration to toggle.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during search functionality toggle.</exception>
        public async Task<bool> DeActivateOrActivateSearchAsync(string id)
        {
            try
            {

                var result = await _mediator.Send(new DeActivateOrActivateSearchCommand(id));
                return result;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Activates the engine immediately for the specified configuration with error handling and logging.
        /// </summary>
        /// <param name="id">The unique identifier of the engine configuration to activate immediately.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during engine activation.</exception>
        public async Task<bool> ActivateEngineImmediatlyAsync(string id)
        {
            try
            {

                var result = await _mediator.Send(new ActivateEngineImmediatlyCommand(id));
                return result;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Generates content based on the provided URL using AI content generation with URL validation, error handling and logging.
        /// </summary>
        /// <param name="url">The URL to generate content from.</param>
        /// <returns>A task that represents the asynchronous operation containing the generated content as a string.</returns>
        /// <exception cref="Exception">Thrown when the URL cannot be reached or an error occurs during content generation.</exception>
        public async Task<string> GenerateContentAsync(string url)
        {
            try
            {

                var urlCanBeReached = await new HttpClient().GetAsync(url);
                if (!urlCanBeReached.IsSuccessStatusCode)
                {
                    throw new Exception("Url cannot be reached");
                }
                var cancelationToken = new CancellationTokenSource().Token;
                var generatedContent = await new ContentGenerator(_configuration, _serviceProvider, _eventAndActivityLog).Generate(url, cancelationToken);
                return generatedContent;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Retrieves all engine configurations from the system with error handling and logging.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing a collection of engine configuration DTOs.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during engine configurations retrieval.</exception>
        public async Task<IEnumerable<EngineConfigurationDto>> GetAllEngineConfigurationsAsync()
        {
            try
            {
                return await _mediator.Send(new GetAllEngineConfigurationsQuery());
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }
    }
}
