using System.Text.Json;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Common.Concrete.Utils
{
    /// <summary>
    /// Utility class for extracting content category data from JSON responses.
    /// </summary>
    public static class ContentCategoryExtractExtensionMethod
    {

        /// <summary>
        /// Extracts content category titles from a JSON response and deserializes it into a ContentCategoryTitleResponse object.
        /// </summary>
        /// <param name="jsonResponse"></param>
        /// <returns></returns>
        public static async Task<ContentCategoryTitleResponse> ContentCategoryExtract(string jsonResponse, IServiceProvider _serviceProvider,
        EventAndActivityLog _eventAndActivityLog)
        {
            var contentCategoryTitleResponse = new ContentCategoryTitleResponse();
            try
            {
                var jsonDoc = JsonDocument.Parse(jsonResponse);
                var root = jsonDoc.RootElement;
                var choices = root.GetProperty("choices");
                foreach (var choice in choices.EnumerateArray())
                {
                    var message = choice.GetProperty("message");
                    var content = message.GetProperty("content").GetString();
                    return JsonSerializer.Deserialize<ContentCategoryTitleResponse>(content);
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = typeof(ContentCategoryExtractExtensionMethod).FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
            return contentCategoryTitleResponse;
        }
    }
}
