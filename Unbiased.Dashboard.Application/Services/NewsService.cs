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
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Application.Services
{
    /// <summary>
    /// Service implementation for news operations providing comprehensive CRUD functionality for generated news management with image support, file upload, AWS integration, validation, error handling and logging.
    /// </summary>
    public sealed class NewsService : INewsService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly AwsCredentials _awsCredentials;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the NewsService class.
        /// </summary>
        /// <param name="mediator">The mediator for handling CQRS operations.</param>
        /// <param name="configuration">The configuration provider for application settings.</param>
        /// <param name="awsOptions">The AWS credentials options for cloud storage operations.</param>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        public NewsService(IMediator mediator, IConfiguration configuration, IOptions<AwsCredentials> awsOptions, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _configuration = configuration;
            _awsCredentials = awsOptions.Value;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Retrieves all generated news with images based on the specified request parameters with error handling and logging.
        /// </summary>
        /// <param name="requestDto">The request DTO containing parameters for retrieving generated news with images.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of generated news with image DTOs.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during news retrieval.</exception>
        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGenerateNewsWithImageAsync(GetGeneratedNewsWithImagePathRequestDto requestDto)
        {
            try
            {
                return await _mediator.Send(new GetGeneratedNewsQuery(requestDto));
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        }

        /// <summary>
        /// Gets the total count of generated news with images based on the specified request parameters with error handling and logging.
        /// </summary>
        /// <param name="requestDto">The request DTO containing parameters for counting generated news with images.</param>
        /// <returns>A task that represents the asynchronous operation containing the total count of generated news with images.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during news count retrieval.</exception>
        public async Task<int> GetAllGenerateNewsWithImageCountAsync(GetGeneratedNewsWithImagePathRequestDto requestDto)
        {
            try
            {
                return await _mediator.Send(new GetAllGeneratedNewsWithImageCountQuery(requestDto));
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a specific generated news item with image by its unique identifier with error handling and logging.
        /// </summary>
        /// <param name="id">The unique identifier of the generated news item to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation containing the generated news with image DTO.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during news retrieval.</exception>
        public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdWithImageAsync(string id)
        {
            try
            {
                return await _mediator.Send(new GetGeneratedNewsByIdWithImageQuery(id));
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        }

        /// <summary>
        /// Deletes a news item by its unique identifier with error handling and logging.
        /// </summary>
        /// <param name="id">The unique identifier of the news item to delete.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during news deletion.</exception>
        public async Task<bool> DeleteNewsAsync(string id)
        {
            try
            {
                return await _mediator.Send(new DeleteGeneretedNewsCommand(id));
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing generated news item with new data and optionally a new image file, including file validation, AWS upload, error handling and logging.
        /// </summary>
        /// <param name="file">The new image file to associate with the news item (optional).</param>
        /// <param name="generateNewsWithImageDto">The news data transfer object containing updated information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when the file is invalid, image upload fails, or an error occurs during news update.</exception>
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
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        }

        /// <summary>
        /// Creates a new news item with the specified data and associated image file, including file validation, AWS upload, error handling and logging.
        /// </summary>
        /// <param name="file">The image file to associate with the news item.</param>
        /// <param name="insertNewsWithImageDto">The news data transfer object containing the news information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when the file is invalid, image upload fails, or an error occurs during news creation.</exception>
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
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        }

        /// <summary>
        /// Validates the insert news DTO and sends the insert command through the mediator.
        /// </summary>
        /// <param name="InsertNewsWithImageDto">The news data transfer object to validate and insert.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when validation fails or an error occurs during insertion.</exception>
        private async Task<bool> ValidateAndInsertNews(InsertNewsWithImageDto InsertNewsWithImageDto)
        {
            var validation = new InsertNewsWithImageDtoValidator().Validate(InsertNewsWithImageDto);
            if (!validation.IsValid)
            {
                throw new Exception(validation.Errors[0].ErrorMessage);
            }
            return await _mediator.Send(new InsertGeneratedNewsWithImageCommand(InsertNewsWithImageDto));
        }

        /// <summary>
        /// Validates the update news DTO and sends the update command through the mediator.
        /// </summary>
        /// <param name="generateNewsWithImageDto">The news data transfer object to validate and update.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when validation fails or an error occurs during update.</exception>
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
