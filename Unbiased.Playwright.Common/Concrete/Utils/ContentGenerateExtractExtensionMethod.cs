using System.Text.Json;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Common.Concrete.Utils
{
    /// <summary>
    /// Utility class for extracting content generation data from JSON responses.
    /// Provides methods to parse and deserialize JSON responses from content generation APIs.
    /// </summary>
    public static class ContentGenerateExtractExtensionMethod
    {
        /// <summary>
        /// Extracts content data from a JSON response and deserializes it into an InsertAllContentDataRequest object.
        /// Handles parsing of complex nested JSON structures from AI services.
        /// </summary>
        /// <param name="jsonResponse">The JSON response from the content generation API.</param>
        /// <returns>A deserialized InsertAllContentDataRequest object containing the content data.</returns>
        public static async Task<InsertAllContentDataRequest> ContentGenerateExtract(string jsonResponse, IServiceProvider _serviceProvider,
        EventAndActivityLog _eventAndActivityLog)
        {
            var insertAllContentDataRequest = new InsertAllContentDataRequest();
            try
            {
                var jsonDoc = JsonDocument.Parse(jsonResponse);
                var root = jsonDoc.RootElement;
                var choices = root.GetProperty("choices");
                foreach (var choice in choices.EnumerateArray())
                {
                    var message = choice.GetProperty("message");
                    var content = message.GetProperty("content").GetString();
                    return JsonSerializer.Deserialize<InsertAllContentDataRequest>(content);
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = typeof(ContentGenerateExtractExtensionMethod).FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
            return insertAllContentDataRequest;
        }
    }
}
