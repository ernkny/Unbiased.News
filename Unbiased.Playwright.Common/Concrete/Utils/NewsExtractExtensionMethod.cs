using System.Text.Json;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Common.Concrete.Utils
{
    /// <summary>
    /// Provides extension methods for extracting news details from a JSON response.
    /// </summary>
    public static class NewsExtractExtensionMethod
    {
        /// <summary>
        /// Extracts news details from a JSON response.
        /// </summary>
        /// <param name="jsonResponse">The JSON response to extract news details from.</param>
        /// <returns>A <see cref="NewsExtractDto"/> containing the extracted news details.</returns>
        public static NewsExtractDto ExtractNewsDetails(string jsonResponse, IEventAndActivityLog _eventAndActivityLog)
        {
            var detailsList = new NewsExtractDto();
            try
            {
                var jsonDoc = JsonDocument.Parse(jsonResponse);
                var root = jsonDoc.RootElement;
                var choices = root.GetProperty("choices");
                foreach (var choice in choices.EnumerateArray())
                {
                    var message = choice.GetProperty("message");
                    var content = message.GetProperty("content").GetString();
                    return JsonSerializer.Deserialize<NewsExtractDto>(content);
                }
            }
            catch (Exception exception)
            {
                _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = typeof(NewsExtractExtensionMethod).FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                }).Wait();
                throw;
            }
            return detailsList;
        }
    }
}
