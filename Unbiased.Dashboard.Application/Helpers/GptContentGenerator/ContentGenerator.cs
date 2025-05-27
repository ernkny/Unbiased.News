using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Application.Helpers.GptContentGenerator
{
    /// <summary>
    /// ContentGenerator class is responsible for generating content based on a provided URL by parsing the content and sending it to the GPT API for analysis and content creation.
    /// </summary>
    public class ContentGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();
        public ContentGenerator(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        ///  Generates content based on the provided URL by parsing the content and sending it to the GPT API for analysis and content creation.
        /// </summary>
        /// <param name="contentUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task<string> Generate(string contentUrl, CancellationToken cancellationToken)
        {
            try
            {
                var detail = await contentParser(contentUrl);
                var responseContent = string.Empty;
                var prompt = $"I am sending you the content of a website. Please create new content in the same language as the original!!, and perform an analysis of it. Present this content and interpret it as if you have researched and written it yourself. Just send your response. A critical point is that your response must be in the same language as the content I provided!!!!.Only focus on main article of content:{detail}";
                var url = _configuration.GetSection("Urls:GptApi").Value;
                var apiKey = _configuration.GetSection("Keys:GptApiKey").Value;
                var requestData = new
                {
                    model = "gpt-4",
                    messages = new[]
                    {
                    new { role = "user", content = prompt }
                    }
                };

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json")
                };
                request.Headers.Add("Authorization", $"Bearer {apiKey}");

                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    throw new Exception($"API returned error: {response.StatusCode}");
                }
                var jsonDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var root = jsonDoc.RootElement;
                var choices = root.GetProperty("choices");
                foreach (var choice in choices.EnumerateArray())
                {
                    var message = choice.GetProperty("message");
                    return message.GetProperty("content").GetString();
                }
                responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        }

        /// <summary>
        ///  Parses the content of a webpage and extracts text from paragraph elements.
        /// </summary>
        /// <param name="contentUrl"></param>
        /// <returns></returns>
        private async Task<string> contentParser(string contentUrl)
        {
            try
            {
                var htmlContent = await _httpClient.GetStringAsync(contentUrl);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                var paragraphNodes = htmlDoc.DocumentNode.SelectNodes("//p");

                var stringBuilder = new StringBuilder();

                if (paragraphNodes != null)
                {
                    foreach (var pNode in paragraphNodes)
                    {
                        stringBuilder.AppendLine(pNode.InnerText);
                    }
                }
                return stringBuilder.ToString();
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }

        }
    }
}
