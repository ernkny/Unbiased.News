using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.Abstract.ExternalServices;

namespace Unbiased.Playwright.Infrastructure.Concrete.ExternalServices
{
    /// <summary>
    /// Service for generating images using the Freepik API.
    /// Implements the IImageGeneratorService interface to provide image generation capabilities.
    /// </summary>
    public class FreepikApiExternalService: IImageGeneratorService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the FreepikApiExternalService class.
        /// </summary>
        /// <param name="httpClient">The HTTP client for making API requests.</param>
        /// <param name="configuration">The application configuration for API URLs and keys.</param>
        /// <param name="mediator">The mediator instance for handling commands and queries.</param>
        public FreepikApiExternalService(HttpClient httpClient, IConfiguration configuration, IMediator mediator)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _mediator = mediator;
        }

        /// <summary>
        /// Sends a request to the Freepik API to initiate image generation.
        /// </summary>
        /// <param name="titleOfGeneration">The prompt text describing the image to generate.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A response DTO containing information about the image generation task.</returns>
        private async Task<FreePikPostImageResponseDto> SendMessageToFreePikForGenerateImage(string titleOfGeneration, CancellationToken cancellationToken = default)
        {
            try
            {
                var url = _configuration.GetSection("Urls:FreePikApi").Value;
                var apiKey = _configuration.GetSection("Keys:FreepikApiKey").Value;

                var requestData = new
                {
                    prompt = titleOfGeneration,
                    aspect_ratio = "standard_3_2",
                    seed = 2147483648
                };

                var json = JsonSerializer.Serialize(requestData);
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                request.Headers.Add("x-freepik-api-key", apiKey);

                var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);


                var result = JsonSerializer.Deserialize<FreePikPostImageResponseDto>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Data?.task_id == null)
                {
                    throw new Exception("Freepik API did not return a valid task_id. Response: " + responseContent);
                }

                var finalResult = await PollFreepikImageUntilReadyAsync(result.Data.task_id, cancellationToken);

                if (finalResult?.Data?.generated?.Any() != true)
                {
                    throw new Exception("Freepik image generation did not complete in expected time.");
                }

                return finalResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Polls the Freepik API to check if the image generation task with the specified taskId has been completed.
        /// It sends repeated GET requests at defined intervals until the image is ready or the maximum number of retries is reached.
        /// Returns the completed image response if available; otherwise, returns null after all retries.
        /// </summary>
        /// <param name="taskId">The task ID returned from the Freepik API when initiating image generation.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation if needed.</param>
        /// <param name="maxRetries">The maximum number of polling attempts (default is 6).</param>
        /// <param name="delayMs">The delay in milliseconds between each polling attempt (default is 2000ms).</param>
        private async Task<FreePikPostImageResponseDto?> PollFreepikImageUntilReadyAsync(string taskId, CancellationToken cancellationToken, int maxRetries = 6, int delayMs = 2000)
        {
            var baseUrl = _configuration.GetSection("Urls:FreePikApi").Value;
            var apiKey = _configuration.GetSection("Keys:FreepikApiKey").Value;
            var fullUrl = $"{baseUrl}/{taskId}";

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                request.Headers.Add("x-freepik-api-key", apiKey);

                var response = await _httpClient.SendAsync(request, cancellationToken);
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                Console.WriteLine($"Polling Attempt {attempt + 1}:");
                Console.WriteLine(content);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<FreePikPostImageResponseDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result?.Data?.status == "COMPLETED" && result.Data.generated?.Any() == true)
                    {
                        return result;
                    }
                }

                await Task.Delay(delayMs, cancellationToken);
            }

            return null;
        }

        /// <summary>
        /// Retrieves the generated image from the Freepik API using the task ID.
        /// </summary>
        /// <param name="taskId">The ID of the image generation task.</param>
        /// <returns>A response DTO containing the URL of the generated image.</returns>
        private async Task<FreePikPostImageResponseDto> GetFreePikForGenerateImage(string taskId)
        {
            try
            {
                var baseUrl = _configuration.GetSection("Urls:FreePikApi").Value;
                var apiKey = _configuration.GetSection("Keys:FreepikApiKey").Value;

                var fullUrl = $"{baseUrl}/{taskId}";

                var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                request.Headers.Add("x-freepik-api-key", apiKey);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<FreePikPostImageResponseDto>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result ?? throw new Exception("Deserialization failed or response is empty.");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the generated image from Freepik.", ex);
            }
        }

        /// <summary>
        /// Generates an image URL based on the provided text prompt using the Freepik API.
        /// The process involves initiating a generation task and then polling for the result.
        /// </summary>
        /// <param name="prompt">The text prompt describing the image to generate.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The URL of the generated image, or null if generation was unsuccessful.</returns>
        public async Task<string> GenerateImageUrlAsync(string prompt, CancellationToken cancellationToken)
        {
            var creationResponse = await SendMessageToFreePikForGenerateImage(prompt,cancellationToken);
            if (creationResponse?.Data?.task_id == null)
                return null;

            await Task.Delay(2000, cancellationToken); 

            var resultUrl = await GetFreePikForGenerateImage(creationResponse.Data.task_id);
            return resultUrl.Data.generated.FirstOrDefault()!;
        }
    }
}
