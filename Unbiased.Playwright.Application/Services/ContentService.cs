using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Unbiased.Playwright.Application.Abstract;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Common.Concrete.Utils;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Domain.Enums;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.Concrete.ExternalServices;

namespace Unbiased.Playwright.Application.Services
{
    public class ContentService : AbstractImageProcess, IContentService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly AwsCredentials _awsCredentials;

        public ContentService(IMediator mediator, IConfiguration configuration, IServiceProvider serviceProvider, IOptions<AwsCredentials> awsOptions): base(awsOptions.Value, mediator, configuration, serviceProvider)
        {
            _mediator = mediator;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _awsCredentials = awsOptions.Value;
        }

        public async Task<bool> AddDailyHoroscopeAsync(HoroscopeDailyDetail horoscopeDetail)
        {
            try
            {
                var result = await _mediator.Send(new InsertDailyHoroscopeCommand(horoscopeDetail));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> GenerateContentAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var subHeadings = await _mediator.Send(new GetAllNoneGeneratedSubHeadingsQuery(), cancellationToken);
                var externalServiceSend = new GptApiExternalService(new HttpClient(), _configuration, _mediator);
                foreach (var item in subHeadings.Take(50))
                {
                    var languageEnum = Enum.Parse<LanguageEnums>(item.Language);
                    var result = await externalServiceSend.SendGeneratedContentPromptAndGetResponse(item.Title, languageEnum, cancellationToken);
                    if (result.IsSuccessStatusCode==true)
                    {
                        await GetGeneratedContentAndSave(result, item.Id, item.ContentCategoryId,cancellationToken);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task<string?> GenerateImageAndSaveAsync(string title, CancellationToken cancellationToken)
        {
            return await SendNewsToApiForGenerateImageAndSaveItAwsAsync(title, ImageGenerationSource.Freepik, cancellationToken);
        }

        public async Task<bool> GenerateSubheadingsAndSaveAsync(CancellationToken cancellationToken)
        {
            try
            {
                var getContentCategoriesAndBaseTitles =await _mediator.Send(new GetAllContentCategoriesQuery());
                var externalServiceSend = new GptApiExternalService(new HttpClient(), _configuration, _mediator);
                foreach (var item in getContentCategoriesAndBaseTitles)
                {
                    var languageEnum = Enum.Parse<LanguageEnums>(item.Language);
                    var response = await externalServiceSend.SendForGetNewContentSubheadingsPromptAndGetResponse(item.CategoryName, languageEnum,cancellationToken);
                    var content = await response.Content.ReadAsStringAsync();
                    var extractContentSubheadings = await ContentCategoryExtractExtensionMethod.ContentCategoryExtract(content);
                    foreach (var subheading in extractContentSubheadings.titles)
                    {
                         await _mediator.Send(new InsertContentSubheadingsCommand(item.Id, subheading));
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        private async Task<bool> GetGeneratedContentAndSave(HttpResponseMessage response, int contentSubheadingId, int contentCategoryId,CancellationToken cancellationToken)
        {
            var convertedData = await ContentGenerateExtractExtensionMethod.ContentGenerateExtract(await response.Content.ReadAsStringAsync());
            convertedData.ContentCategoryId = contentCategoryId;
            convertedData.ContentSubHeadingId = contentSubheadingId;
            var imageFile = await GenerateImageAndSaveAsync(convertedData.ImagePrompt, cancellationToken);
            if (imageFile is null)
            {
                imageFile = @"https://unbiasedbucket.s3.eu-north-1.amazonaws.com/Pictures/noimage.png";
            }
            else
            {
                convertedData.ImagePath = imageFile;
            }
            var result= await _mediator.Send(new InsertGeneratedContentWithDetailCommand(convertedData), cancellationToken);
            return result;
        }
    }
}
