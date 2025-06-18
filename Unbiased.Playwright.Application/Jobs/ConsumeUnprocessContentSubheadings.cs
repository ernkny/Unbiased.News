using MediatR;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using Unbiased.Playwright.Application.Exceptions.Custom;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Application.Jobs
{
    /// <summary>
    /// Quartz job responsible for processing unprocessed content subheadings.
    /// This job identifies subheadings that haven't been processed yet and 
    /// generates content for them using the content service.
    /// </summary>
    [DisallowConcurrentExecution]
    public class ConsumeUnprocessContentSubheadings : IJob
    {
        private readonly IContentService _contentService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the ConsumeUnprocessContentSubheadings class.
        /// </summary>
        /// <param name="contentService">The content service for generating content.</param>
        public ConsumeUnprocessContentSubheadings(IContentService contentService, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _contentService = contentService;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Executes the job to process unprocessed content subheadings.
        /// Handles rate limiting by implementing retry logic for too many requests errors.
        /// </summary>
        /// <param name="context">The context in which the job is being executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _contentService.GenerateContentAsync(context.CancellationToken);
            }
            catch (Exception exception) when (exception.Message.Contains("TooManyRequests"))
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
            catch (TooManyRequestsException exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                await Task.Delay(TimeSpan.FromMinutes(1));
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
