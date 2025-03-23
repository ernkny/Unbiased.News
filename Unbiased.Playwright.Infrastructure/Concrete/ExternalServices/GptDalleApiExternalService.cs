using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.Abstract.ExternalServices;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Infrastructure.Concrete.ExternalServices
{
    /// <summary>
    /// External service for interacting with the GPT DALL-E API.
    /// </summary>
    public class GptDalleApiExternalService : AbstractEventAndActivityLog, IImageGeneratorService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GptDalleApiExternalService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="mediator">The mediator instance.</param>
        /// <param name="serviceProvider">The service provider instance.</param>
        public GptDalleApiExternalService(HttpClient httpClient, IConfiguration configuration, IMediator mediator, IServiceProvider serviceProvider)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _mediator = mediator;
        }

        /// <summary>
        /// Generates an image URL based on the provided text prompt using the GPT DALL-E API.
        /// </summary>
        /// <param name="prompt">The text prompt describing the image to generate.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The URL of the generated image, or null if generation was unsuccessful.</returns>
        public async Task<string?> GenerateImageUrlAsync(string prompt, CancellationToken cancellationToken)
        {
            var result = await GetImageDataFromGpt(prompt, cancellationToken);
            return result?.Data?.FirstOrDefault()?.Url;
        }

        /// <summary>
        /// Retrieves image data from the GPT DALL-E API based on the provided title.
        /// </summary>
        /// <param name="titleOfNewsForGenerateImage">The title of the news article to generate an image for.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The generated image data as a <see cref="DalleImageGenerateResponseDto"/> instance.</returns>
        public async Task<DalleImageGenerateResponseDto> GetImageDataFromGpt(string titleOfNewsForGenerateImage, CancellationToken cancellationToken)
        {
            var result = new DalleImageGenerateResponseDto();
            var imageGeneratePrompt = $@"'{titleOfNewsForGenerateImage}' bu başlığa uygun bir banner oluştur ve bu oluşturacağın resim üzerinde hiçbir şekilde yazı olmasın, Herhangi bir ülke bayrağı belirtmesin. Banner ana konuya odaklı bir banner olsun";
            try
            {
                var url = _configuration.GetSection("Urls:GptDalleApi").Value;
                var apiKey = _configuration.GetSection("Keys:GptApiKey").Value;
                var requestData = new
                {
                    model = "dall-e-3",
                    prompt = imageGeneratePrompt,
                    n = 1,
                    size = "1792x1024"
                };
                var bisi = JsonSerializer.Serialize(requestData);
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
                    var message = ($"API returned error: {response.StatusCode}");
                    var eventLog = new EventLog()
                    {
                        EventDate = DateTime.UtcNow,
                        EventSeverity = "Error",
                        Message = message,
                        EventType = "GptDalleApiExternalService",

                    };
                    await SendEventLogToQueue(eventLog, _serviceProvider);
                    return null;

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
