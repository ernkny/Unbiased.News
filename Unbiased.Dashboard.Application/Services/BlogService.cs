using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Application.Validators;
using Unbiased.Dashboard.Common.Concrete.Helpers;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Domain.Model.Aws;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Blogs;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Blogs;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Application.Services
{
	/// <summary>
	/// Service implementation for blog operations providing comprehensive CRUD functionality for blog management with file upload, AWS integration, validation, error handling and logging.
	/// </summary>
	public sealed class BlogService : IBlogService
	{
		private readonly IMediator _mediator;
		private readonly IConfiguration _configuration;
		private readonly AwsCredentials _awsCredentials;
		private readonly IServiceProvider _serviceProvider;
		private readonly IEventAndActivityLog _eventAndActivityLog;

		/// <summary>
		/// Initializes a new instance of the BlogService class.
		/// </summary>
		/// <param name="mediator">The mediator for handling CQRS operations.</param>
		/// <param name="configuration">The configuration provider for application settings.</param>
		/// <param name="awsOptions">The AWS credentials options for cloud storage operations.</param>
		/// <param name="serviceProvider">The service provider for dependency injection.</param>
		public BlogService(IMediator mediator, IConfiguration configuration, IOptions<AwsCredentials> awsOptions, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
		{
			_mediator = mediator;
			_configuration = configuration;
			_awsCredentials = awsOptions.Value;
			_serviceProvider = serviceProvider;
			_eventAndActivityLog = eventAndActivityLog;
		}

		/// <summary>
		/// Retrieves all blogs based on the specified request parameters with pagination and filtering, including error handling and logging.
		/// </summary>
		/// <param name="blogRequestDto">The request DTO containing pagination and filtering parameters.</param>
		/// <returns>A task that represents the asynchronous operation containing a collection of blog DTOs.</returns>
		/// <exception cref="Exception">Thrown when an error occurs during blog retrieval.</exception>
		public async Task<IEnumerable<BlogDto>> GetAllBlogsAsync(BlogRequestDto blogRequestDto)
		{
			try
			{
				return await _mediator.Send(new GetAllBlogsQuery(blogRequestDto));
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Gets the total count of blogs based on the specified filtering criteria with error handling and logging.
		/// </summary>
		/// <param name="blogRequestDto">The request DTO containing filtering parameters.</param>
		/// <returns>A task that represents the asynchronous operation containing the total count of blogs.</returns>
		/// <exception cref="Exception">Thrown when an error occurs during blog count retrieval.</exception>
		public async Task<int> GetAllBlogsCountAsync(BlogRequestDto blogRequestDto)
		{
			try
			{
				return await _mediator.Send(new GetAllBlogsCountQuery(blogRequestDto));
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Retrieves a specific blog by its unique identifier including associated image data with error handling and logging.
		/// </summary>
		/// <param name="id">The unique identifier of the blog to retrieve.</param>
		/// <returns>A task that represents the asynchronous operation containing the blog DTO with image.</returns>
		/// <exception cref="Exception">Thrown when an error occurs during blog retrieval.</exception>
		public async Task<BlogDto> GetBlogByIdWithImageAsync(string id)
		{
			try
			{
				return await _mediator.Send(new GetBlogByIdQuery(id));
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Deletes a blog by its unique identifier with error handling and logging.
		/// </summary>
		/// <param name="id">The unique identifier of the blog to delete.</param>
		/// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
		/// <exception cref="Exception">Thrown when an error occurs during blog deletion.</exception>
		public async Task<bool> DeleteBlogAsync(string id)
		{
			try
			{
				return await _mediator.Send(new DeleteBlogCommand(id));
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Creates a new blog with the specified data and associated image file, including file validation, AWS upload, error handling and logging.
		/// </summary>
		/// <param name="blogRequestDto">The blog data transfer object containing the blog information.</param>
		/// <param name="UserId">The unique identifier of the user creating the blog.</param>
		/// <param name="file">The image file to associate with the blog.</param>
		/// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
		/// <exception cref="Exception">Thrown when the file is invalid, image upload fails, or an error occurs during blog creation.</exception>
		public async Task<bool> InsertBlogAsync(InsertBlogDtoRequest blogRequestDto, int UserId, IFormFile file)
		{
			try
			{
				if (!new FormFileValidation().IsValidFile(file))
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

				blogRequestDto.Path = resultOfPicture;
				return await _mediator.Send(new InsertBlogCommand(blogRequestDto, UserId));
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Updates an existing blog with new data and optionally a new image file, including file validation, AWS upload, error handling and logging.
		/// </summary>
		/// <param name="blogRequestDto">The blog data transfer object containing updated information.</param>
		/// <param name="file">The new image file to associate with the blog (optional).</param>
		/// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
		/// <exception cref="Exception">Thrown when the file is invalid, image upload fails, or an error occurs during blog update.</exception>
		public async Task<bool> UpdateBlogAsync(UpdateBlogDtoRequest blogRequestDto, IFormFile file)
		{
			try
			{
				if (file == null)
				{
					return await _mediator.Send(new UpdateBlogCommand(blogRequestDto));
				}

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

				blogRequestDto.Path = resultOfPicture;
				return await _mediator.Send(new UpdateBlogCommand(blogRequestDto));
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}
	}
}
