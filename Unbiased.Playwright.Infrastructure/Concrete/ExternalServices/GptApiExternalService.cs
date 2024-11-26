using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using Unbiased.Playwright.Common.Concrete.Utils;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;

namespace Unbiased.Playwright.Infrastructure.Concrete.ExternalServices
{
    public class GptApiExternalService
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

        public async Task<NewsExtractDto> SendCombinedNewsDetailToGpt(string DetailIOfNews)
        {
            var result= new NewsExtractDto();
            try
            {
                var url = _configuration.GetSection("Urls:GptApi").Value;
                var apiKey = _configuration.GetSection("Keys:GptApiKey").Value;
                var requestData = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                    new { role = "user", content = $"Bir gazeteci gibi bu paylaştığım haberi oku ve bir yeni haber olarak ilk satırı başlığı olacak şekilde bana analiz edip bir haber olarak içerik çıkar. Bunu senin abonelerin okuyacakmış gibi haber çıkart aynı zamanda haberi analiz edip yorumunu ekle. Birde Title: ve Detail: olacak şekilde döner misin yazıyı? apiden okuyacağım -----{DetailIOfNews}----- " }
                }
                };

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json")
                };
                request.Headers.Add("Authorization", $"Bearer {apiKey}");

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API returned error: {response.StatusCode}");
                }
                if (response.IsSuccessStatusCode)
                {
                    await _mediator.Send(new AddOpenApiResponseCommand(await response.Content.ReadAsStringAsync()));
                    result= NewsExtractExtensionMethod.ExtractNewsDetails(await response.Content.ReadAsStringAsync());

                }

                string responseContent = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }
    }
}

