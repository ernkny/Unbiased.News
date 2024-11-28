using System.Text.Json;
using Unbiased.Playwright.Domain.DTOs;

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
        public static NewsExtractDto ExtractNewsDetails(string jsonResponse)
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

                    var title = ExtractBetween(content, "**Title:**", "\n\n");
                    var detail = ExtractBetween(content, "**Detail:**", "\n\n");

                    return new NewsExtractDto { Title = title, Detail = detail };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing JSON: " + ex.Message);
            }
            return detailsList;
        }

        /// <summary>
        /// Extracts a substring from a source string, starting from a specified start string and ending at a specified end string.
        /// </summary>
        /// <param name="source">The source string to extract from.</param>
        /// <param name="start">The start string to begin extraction from.</param>
        /// <param name="end">The end string to end extraction at.</param>
        /// <returns>The extracted substring, or null if the start or end strings are not found.</returns>
        private static string ExtractBetween(string source, string start, string end)
        {
            var startIndex = source.IndexOf(start) + start.Length;
            var endIndex = source.IndexOf(end, startIndex);
            if (startIndex < start.Length || endIndex == -1)
            {
                return null;
            }
            return source.Substring(startIndex, endIndex - startIndex).Trim();
        }
    }
}
