using MediatR;
using Microsoft.Extensions.Configuration;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Common.Concrete.Helper;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Domain.Enums;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.Concrete.ExternalServices;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;


namespace Unbiased.Playwright.Application.Services
{
    /// <summary>
    /// NewsService is responsible for handling news-related operations.
    /// It implements the INewsService interface.
    /// </summary>
    public sealed class NewsService : INewsService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly AwsCredentials _awsCredentials;

        /// <summary>
        /// Initializes a new instance of the NewsService class.
        /// </summary>
        /// <param name="mediator">The mediator instance.</param>
        /// <param name="configuration">The configuration instance.</param>
        public NewsService(IMediator mediator, IConfiguration configuration, IServiceProvider serviceProvider, IOptions<AwsCredentials> awsOptions)
        {
            _mediator = mediator;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _awsCredentials = awsOptions.Value;
        }

        /// <summary>
        /// Adds a new news item asynchronously.
        /// </summary>
        /// <param name="addNewsDto">The insert news DTO.</param>
        /// <returns>A task representing the asynchronous operation, containing the GUID of the added news item.</returns>
        public async Task<Guid> AddNewsAsync(InsertNewsDto addNewsDto)
        {
            var result = await _mediator.Send(new AddNewsCommand(addNewsDto));
            return result;
        }

        /// <summary>
        /// Sends news to the API for generation asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation, containing a boolean indicating success.</returns>
        public async Task<bool> SendNewsToApiForGenerateAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var combinedNews = await _mediator.Send(new GetAllNewsCombinedDetailsQuery(), cancellationToken);
                var externalServiceSend = new GptApiExternalService(new HttpClient(), _configuration, _mediator);
                var result = await GenerateNewsWithApiAsync(combinedNews, cancellationToken, externalServiceSend);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> GenerateImagesWhenAllNewsHasGeneratedAsync(CancellationToken cancellationToken)
        {
            var isDone = false;
            while (!isDone)
            {
                var images = await _mediator.Send(new GetImagesWithNoneHasGeneratedQuery(), cancellationToken);
                _ = images.Count() == 0 ? isDone = true : isDone = false;
                foreach (var item in images)
                {
                    var imageFile = await SendNewsToApiForGenerateImageAndSaveItAwsAsync(item.Title, cancellationToken);
                    if (imageFile is not null)
                    {
                        await _mediator.Send(new InsertGeneratedImageCommand(new InsertNewsImageDto
                        {
                            MatchId = item.Id,
                            filePath = imageFile
                        }), cancellationToken);
                    }
                }
            }

            return true;
        }

        public async Task<IEnumerable<GeneratedNewsWithNoneImageDto>> GenerateImagesAsyncWithNoneHasGenerated(CancellationToken cancellationToken)
        {
            var images = await _mediator.Send(new GetImagesWithNoneHasGeneratedQuery(), cancellationToken);
            return images;
        }

        public async Task<bool> GenerateNewsWithApiAsync(IEnumerable<GeneratedNewsDto> combinedNews, CancellationToken cancellationToken, GptApiExternalService externalServiceSend)
        {
            try
            {
                foreach (var item in combinedNews)
                {
                    LanguageEnums language = (LanguageEnums)Enum.Parse(typeof(LanguageEnums), item.Language);
                    var result = await externalServiceSend.SendCombinedNewsDetailToGpt(item.CombinedDetails, language, cancellationToken);
                    if (result == null) throw new ArgumentException("Error");

                    var generatedNews = new News
                    {
                        Detail = result.Detail,
                        Title = result.Title,
                        MatchId = item.MatchId,
                        CategoryId = item.CategoryId,
                        Language = item.Language

                    };
                    if (!string.IsNullOrEmpty(result.Title) || !string.IsNullOrEmpty(result.Detail))
                    {
                        var saveGeneratedNews = await _mediator.Send(new AddGeneratedNewsCommand(generatedNews), cancellationToken);
                        var imageMatchValidate = await _mediator.Send(new GetNewsImageWithMatchIdQuery(item.MatchId), cancellationToken);
                        if (imageMatchValidate && saveGeneratedNews)
                        {

                            var imageFile = await SendNewsToApiForGenerateImageAndSaveItAwsAsync(result.Title, cancellationToken);
                            if (imageFile is not null)
                            {
                                await _mediator.Send(new InsertGeneratedImageCommand(new InsertNewsImageDto
                                {
                                    MatchId = item.MatchId,
                                    filePath = imageFile
                                }), cancellationToken);
                            }
                        }
                    }

                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private async Task<string> SendNewsToApiForGenerateImageAsync(string Title, CancellationToken cancellationToken)
        {
            var externalServiceImageSend = new GptDalleApiExternalService(new HttpClient(), _configuration, _mediator, _serviceProvider);
            var generatedImage = await externalServiceImageSend.GetImageDataFromGpt(Title, cancellationToken);
            return generatedImage != null ? await new SaveGeneratedImage(_configuration).SaveGeneratedImageToFile(generatedImage.Data.First().Url, Title, cancellationToken) : null;
        }

        private async Task<string> SendNewsToApiForGenerateImageAndSaveItAwsAsync(string Title, CancellationToken cancellationToken)
        {
            var externalServiceImageSend = new GptDalleApiExternalService(new HttpClient(), _configuration, _mediator, _serviceProvider);
            var generatedImage = await externalServiceImageSend.GetImageDataFromGpt(Title, cancellationToken);
            return generatedImage != null ? await new SaveGeneratedImageToAws(_awsCredentials!).GetFileFromGptAndUploadFileAsync(_awsCredentials.BucketName, _configuration.GetSection("Paths:AwsFilePath").Value, generatedImage.Data.First().Url,cancellationToken) : null;
        }
    }
}
