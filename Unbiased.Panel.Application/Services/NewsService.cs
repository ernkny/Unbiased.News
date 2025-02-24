using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Application.Validators;
using Unbiased.Dashboard.Common.Concrete.Helpers;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Domain.Model.Aws;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.News;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.News;

namespace Unbiased.Dashboard.Application.Services
{
    public sealed class NewsService : INewsService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly AwsCredentials _awsCredentials;

        public NewsService(IMediator mediator, IConfiguration configuration, IOptions<AwsCredentials> awsOptions)
        {
            _mediator = mediator;
            _configuration = configuration;
            _awsCredentials = awsOptions.Value;
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGenerateNewsWithImageAsync(GetGeneratedNewsWithImagePathRequestDto requestDto)
        {
            try
            {
                return await _mediator.Send(new GetGeneratedNewsQuery(requestDto));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> GetAllGenerateNewsWithImageCountAsync(GetGeneratedNewsWithImagePathRequestDto requestDto)
        {
            try
            {
                return await _mediator.Send(new GetAllGeneratedNewsWithImageCountQuery(requestDto));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdWithImageAsync(string id)
        {
            try
            {
                return await _mediator.Send(new GetGeneratedNewsByIdWithImageQuery(id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateGeneratedNewsWithImageAsync(IFormFile? file, UpdateGeneratedNewsDto generateNewsWithImageDto)
        {
            try
            {
                if (file is null)
                {
                    return await ValidateAndUpdateNews(generateNewsWithImageDto);
                }

                if (!new FormFileValidation().IsValidFile(file))
                {
                    throw new Exception("File is not valid");
                }

                var fileUpload = new SaveNewsImageToAws(_awsCredentials!);
                var convertedFile = await new FileConvertToByteArray().ConvertToByteArray(file);
                var resultOfPicture = await fileUpload.UploadFileToAws(_configuration.GetSection("Paths:AwsFilePath").Value, _awsCredentials.BucketName, convertedFile);

                if (resultOfPicture is null)
                {
                    throw new Exception("Image not found");
                }

                generateNewsWithImageDto.ImagePath = resultOfPicture;
                return await ValidateAndUpdateNews(generateNewsWithImageDto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Inserts news with an image asynchronously.
        /// </summary>
        /// <param name="file">The image file to upload.</param>
        /// <param name="insertNewsWithImageDto">The DTO containing news data.</param>
        /// <returns>A boolean indicating whether the insertion was successful.</returns>
        public async Task<bool> InsertNewsWithImageAsync(IFormFile file, InsertNewsWithImageDto insertNewsWithImageDto)
        {
            try
            {
                if (file != null && !new FormFileValidation().IsValidFile(file))
                {
                    throw new Exception("File is not valid");
                }

                var fileUpload = new SaveNewsImageToAws(_awsCredentials);
                var convertedFile = await new FileConvertToByteArray().ConvertToByteArray(file);
                var resultOfPicture = await fileUpload.UploadFileToAws(_configuration.GetSection("Paths:AwsFilePath").Value, _awsCredentials.BucketName, convertedFile);

                if (resultOfPicture == null)
                {
                    throw new Exception("Image not found");
                }

                insertNewsWithImageDto.ImagePath = resultOfPicture;
                return await ValidateAndInsertNews(insertNewsWithImageDto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> ValidateAndInsertNews(InsertNewsWithImageDto InsertNewsWithImageDto)
        {
            var validation = new InsertNewsWithImageDtoValidator().Validate(InsertNewsWithImageDto);
            if (!validation.IsValid)
            {
                throw new Exception(validation.Errors[0].ErrorMessage);
            }
            return await _mediator.Send(new InsertGeneratedNewsWithImageCommand(InsertNewsWithImageDto));
        }

        private async Task<bool> ValidateAndUpdateNews(UpdateGeneratedNewsDto generateNewsWithImageDto)
        {
            var validation = new UpdateGeneratedNewsWithImageDtoValidator().Validate(generateNewsWithImageDto);
            if (!validation.IsValid)
            {
                throw new Exception(validation.Errors[0].ErrorMessage);
            }
            return await _mediator.Send(new UpdateGeneratedNewsWithImageCommand(generateNewsWithImageDto));
        }
    }
}
