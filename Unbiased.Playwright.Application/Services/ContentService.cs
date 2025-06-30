using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Unbiased.Playwright.Application.Abstract;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Application.Playwright.Concrete.Playwright.ImageScrapping;
using Unbiased.Playwright.Common.Concrete.Helper;
using Unbiased.Playwright.Common.Concrete.Utils;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Domain.Enums;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.Concrete.ExternalServices;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Application.Services
{
    /// <summary>
    /// Provides content generation and management services.
    /// </summary>
    public class ContentService : AbstractImageProcess, IContentService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;
        private readonly IPlaywright _playwright;
        private readonly AwsCredentials _awsCredentials;
        private readonly WebRootPathOptions _webRootOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentService"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="configuration"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="awsOptions"></param>
        public ContentService(IMediator mediator, IConfiguration configuration, IServiceProvider serviceProvider, IOptions<AwsCredentials> awsOptions, IEventAndActivityLog eventAndActivityLog, IPlaywright playwright, WebRootPathOptions webRootOptions) : base(awsOptions.Value, mediator, configuration, serviceProvider, eventAndActivityLog)
        {
            _mediator = mediator;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
            _playwright = playwright;
            _awsCredentials = awsOptions.Value;
            _webRootOptions = webRootOptions;
        }

        /// <summary>
        ///  Adds a daily horoscope detail to the database.
        /// </summary>
        /// <param name="horoscopeDetail"></param>
        /// <returns></returns>
        public async Task<bool> AddDailyHoroscopeAsync(HoroscopeDailyDetail horoscopeDetail)
        {
            try
            {
                var result = await _mediator.Send(new InsertDailyHoroscopeCommand(horoscopeDetail));
                return result;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        ///  Generates content for subheadings and saves it to the database.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> GenerateContentAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var subHeadings = await _mediator.Send(new GetAllNoneGeneratedSubHeadingsQuery(), cancellationToken);
                var externalServiceSend = new GptApiExternalService(new HttpClient(), _configuration, _mediator, _serviceProvider, _eventAndActivityLog);
                foreach (var item in subHeadings.Take(50))
                {
                    var languageEnum = Enum.Parse<LanguageEnums>(item.Language);
                    var result = await externalServiceSend.SendGeneratedContentPromptAndGetResponse(item.Title, languageEnum, cancellationToken);
                    if (result.IsSuccessStatusCode == true)
                    {
                        await GetGeneratedContentAndSave(result, item.Title, item.Id, item.ContentCategoryId, item.CategoryName, cancellationToken);
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        ///  Generates an image based on the provided title and saves it to AWS S3.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<string?> GenerateImageAndSaveAsync(string title, CancellationToken cancellationToken)
        {
            return await SendNewsToApiForGenerateImageAndSaveItAwsAsync(title, ImageGenerationSource.Freepik, cancellationToken);
        }

        /// <summary>
        ///  Generates subheadings for content categories and saves them to the database.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> GenerateSubheadingsAndSaveAsync(CancellationToken cancellationToken)
        {
            try
            {
                var getContentCategoriesAndBaseTitles = await _mediator.Send(new GetAllContentCategoriesQuery());
                var externalServiceSend = new GptApiExternalService(new HttpClient(), _configuration, _mediator, _serviceProvider, _eventAndActivityLog);
                foreach (var item in getContentCategoriesAndBaseTitles)
                {
                    var languageEnum = Enum.Parse<LanguageEnums>(item.Language);
                    var response = await externalServiceSend.SendForGetNewContentSubheadingsPromptAndGetResponse(item.CategoryName, languageEnum, cancellationToken);
                    var content = await response.Content.ReadAsStringAsync();
                    var extractContentSubheadings = await ContentCategoryExtractExtensionMethod.ContentCategoryExtract(content, _serviceProvider, _eventAndActivityLog);
                    foreach (var subheading in extractContentSubheadings.titles)
                    {
                        await _mediator.Send(new InsertContentSubheadingsCommand(item.Id, subheading));
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        ///  Retrieves generated content from the response and saves it to the database.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="contentSubheadingId"></param>
        /// <param name="contentCategoryId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> GetGeneratedContentAndSave(HttpResponseMessage response, string itemTitle,int contentSubheadingId, int contentCategoryId, string categoryName, CancellationToken cancellationToken)
        {
            try
            {
                var imageFile = string.Empty;
                var convertedData = await ContentGenerateExtractExtensionMethod.ContentGenerateExtract(await response.Content.ReadAsStringAsync(), _serviceProvider, _eventAndActivityLog);
                convertedData.ContentCategoryId = contentCategoryId;
                convertedData.ContentSubHeadingId = contentSubheadingId;
                var scrapedImage = await GetImageWithTitleScrapping.GetImageWithTitle(itemTitle, _playwright, _eventAndActivityLog);
                var saveImageToAWs = new SaveGeneratedImageToAws(_awsCredentials!, _eventAndActivityLog);
                if (string.IsNullOrEmpty(scrapedImage))
                {
                    var garetHeavyFont = Path.Combine(_webRootOptions.WebRootPath, $"{_configuration["StaticFiles:FontsPath"]}Garet-Heavy.ttf");
                    var garetBookFont = Path.Combine(_webRootOptions.WebRootPath, $"{_configuration["StaticFiles:FontsPath"]}Garet-Book.ttf");
                    var ImagePath = Path.Combine(_webRootOptions.WebRootPath, $"{_configuration["StaticFiles:ImagePath"]}KırmızıBanner.png");
                    using (var stream = File.OpenRead(ImagePath))
                    {
                        var title = itemTitle.Length > 60 ? itemTitle.Substring(0, 60) + "..." :itemTitle;
                        var resultGeneratedImage = await GenerateImageBannerWithTitle.ApplyTextOnImageAsync(stream, categoryName.ToUpper(), itemTitle, garetHeavyFont, garetBookFont);
                        imageFile = await saveImageToAWs.SaveGeneratedBannerImageToAws(resultGeneratedImage, _awsCredentials.BucketName, _configuration.GetSection("Paths:AwsFilePath").Value);
                    }
                }
                else
                {
                    imageFile = await saveImageToAWs.GetFileFromGptAndUploadFileAsync(
                                  _awsCredentials.BucketName,
                                  _configuration.GetSection("Paths:AwsFilePath").Value,
                                  scrapedImage,
                                  cancellationToken
                              );
                }
                convertedData.ImagePath = string.IsNullOrEmpty(imageFile) ? @"https://unbiasedbucket.s3.eu-north-1.amazonaws.com/Pictures/noimage.png" : imageFile;
                var result = await _mediator.Send(new InsertGeneratedContentWithDetailCommand(convertedData), cancellationToken);
                return result;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }

        }
    }
}
