using System.Text.Json;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Common.Concrete.Utils
{
    public static class NewsExtractExtensionMethod
    {
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
