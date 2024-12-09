using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using Unbiased.Playwright.Common.Concrete.Utils;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;

namespace Unbiased.Playwright.Infrastructure.Concrete.ExternalServices
{
    /// <summary>
    /// External service for interacting with the GPT API.
    /// </summary>

    public sealed class GptApiExternalService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        public GptApiExternalService(HttpClient httpClient, IConfiguration configuration, IMediator mediator)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _mediator = mediator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GptApiExternalService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client instance.</param>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="mediator">The mediator instance.</param>
        public async Task<NewsExtractDto> SendCombinedNewsDetailToGpt(string DetailIOfNews, CancellationToken cancellationToken)
        {
            var result = new NewsExtractDto();
            var newsAnalysis = $@"Bir gazeteci gibi bu paylaştığım haberi oku ve bana analiz edip bir haber olarak içerik çıkar. Bunu senin abonelerin okuyacakmış gibi haber çıkart aynı zamanda haberi analiz edip yorumlarını ekle. Haberin kesinlikle başlığı olmadlı ve içeriği olmalı. Haber içeriği:  -----{DetailIOfNews}-----";

            var prompt = $@"
            {newsAnalysis}

            KESINLIKLE yanıtını aşağıdaki formatta ver:

            {{
                ""Title"": ""[Haber başlığı]"",
                ""Detail"": ""[Haber detayı ve analizi]""
            }}
            ";

            try
            {
                var url = _configuration.GetSection("Urls:GptApi").Value;
                var apiKey = _configuration.GetSection("Keys:GptApiKey").Value;
                var requestData = new
                {
                    model = "gpt-4o-mini",
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
                if (response.IsSuccessStatusCode)
                {
                    await _mediator.Send(new AddOpenApiResponseCommand(await response.Content.ReadAsStringAsync()));
                    result = NewsExtractExtensionMethod.ExtractNewsDetails(await response.Content.ReadAsStringAsync());

                }

                string responseContent = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (HttpRequestException e)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }
    }
}

