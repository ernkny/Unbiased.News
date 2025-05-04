using System.Text.Json;
using Unbiased.Playwright.Domain.DTOs;

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
        public static async Task<InsertAllContentDataRequest> ContentGenerateExtract(string jsonResponse)
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
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing JSON: " + ex.Message);
            }
            return insertAllContentDataRequest;
        }
    }
}
