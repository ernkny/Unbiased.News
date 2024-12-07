using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using System.Threading;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;

namespace Unbiased.Playwright.Infrastructure.Concrete.ExternalServices
{
    public class GptDalleApiExternalService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public GptDalleApiExternalService(HttpClient httpClient, IConfiguration configuration, IMediator mediator)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<DalleImageGenerateResponseDto> GetImageDataFromGpt(string titleOfNewsForGenerateImage, CancellationToken cancellationToken)
        {
            var result = new DalleImageGenerateResponseDto();
            var imageGeneratePrompt = $@"'{titleOfNewsForGenerateImage}' bu başlığa uygun bir banner oluştur ve üzerinde hiçbir şekilde yazı olmasın, Herhangi bir ülke bayrağı belirtmesin. Banner ana konuya odaklı bir banner olsun";
            try
            {
                var url = _configuration.GetSection("Urls:GptDalleApi").Value;
                var apiKey = _configuration.GetSection("Keys:GptApiKey").Value;
                var requestData = new
                {
                    model = "dall-e-3",
                    prompt=imageGeneratePrompt,
                    n=1,
                    size= "1792x1024"
                };
                var bisi= JsonSerializer.Serialize(requestData);
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
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    result = JsonSerializer.Deserialize<DalleImageGenerateResponseDto>(await response.Content.ReadAsStringAsync(), options);

                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
