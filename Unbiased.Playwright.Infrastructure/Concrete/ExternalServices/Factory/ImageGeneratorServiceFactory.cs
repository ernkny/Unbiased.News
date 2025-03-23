using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Unbiased.Playwright.Domain.Enums;
using Unbiased.Playwright.Infrastructure.Abstract.ExternalServices;

namespace Unbiased.Playwright.Infrastructure.Concrete.ExternalServices.Factory
{
    /// <summary>
    /// Factory class for creating image generator service instances based on the specified source.
    /// Uses the factory pattern to create the appropriate implementation of IImageGeneratorService.
    /// </summary>
    public static class ImageGeneratorServiceFactory
    {
       
        /// <summary>
        /// Creates an instance of IImageGeneratorService based on the specified image generation source.
        /// </summary>
        /// <param name="source">The image generation source to use (e.g., Dalle, Freepik).</param>
        /// <param name="provider">The service provider for resolving dependencies.</param>
        /// <returns>An implementation of IImageGeneratorService for the specified source.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unsupported source is specified.</exception>
        public static IImageGeneratorService Create(ImageGenerationSource source, IServiceProvider provider)
        {
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(); 
            return source switch
            {
                ImageGenerationSource.Dalle => new GptDalleApiExternalService(
                    httpClient,
                    provider.GetRequiredService<IConfiguration>(),
                    provider.GetRequiredService<IMediator>(),
                    provider
                ),
                ImageGenerationSource.Freepik => new FreepikApiExternalService(
                    httpClient,
                    provider.GetRequiredService<IConfiguration>(),
                    provider.GetRequiredService<IMediator>()
                ),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
            };
        }
    }
}
