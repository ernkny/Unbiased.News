using MediatR;
using Microsoft.Extensions.Configuration;
using Unbiased.Playwright.Common.Concrete.Helper;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Enums;
using Unbiased.Playwright.Infrastructure.Concrete.ExternalServices.Factory;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Application.Abstract
{
    /// <summary>
    /// Abstract base class for image processing operations.
    /// Provides a template for generating images and saving them to AWS storage.
    /// Classes that extend this must implement the GenerateImageAndSaveAsync method.
    /// </summary>
    public abstract class AbstractImageProcess
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly AwsCredentials _awsCredentials;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the AbstractImageProcess class.
        /// </summary>
        /// <param name="awsCredentials">AWS credentials for S3 access.</param>
        /// <param name="mediator">The mediator instance for handling requests.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="serviceProvider">The service provider for dependency resolution.</param>
        public AbstractImageProcess(AwsCredentials awsCredentials, IMediator mediator, IConfiguration configuration, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _awsCredentials = awsCredentials;
            _mediator = mediator;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// This method must be implemented by subclasses.
        /// </summary>
        /// <param name="title">The title to generate an image for.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The saved image URL or null if failed.</returns>
        public abstract Task<string?> GenerateImageAndSaveAsync(string title, CancellationToken cancellationToken);

        /// <summary>
        /// Protected helper method to generate an image and upload it to AWS.
        /// </summary>
        /// <param name="title">The title for generating the image.</param>
        /// <param name="source">The image generation source (e.g., GPT, DALL-E).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Saved image URL or null.</returns>
        protected async Task<string?> SendNewsToApiForGenerateImageAndSaveItAwsAsync(string title, ImageGenerationSource source, CancellationToken cancellationToken)
        {
            var imageGeneratorService = ImageGeneratorServiceFactory.Create(source, _serviceProvider);
            var imageUrl = await imageGeneratorService.GenerateImageUrlAsync(title, cancellationToken);

            if (string.IsNullOrEmpty(imageUrl))
                return null;

            return await new SaveGeneratedImageToAws(_awsCredentials!, _eventAndActivityLog).GetFileFromGptAndUploadFileAsync(
                _awsCredentials.BucketName,
                _configuration.GetSection("Paths:AwsFilePath").Value,
                imageUrl,
                cancellationToken
            );
        }
    }
}
