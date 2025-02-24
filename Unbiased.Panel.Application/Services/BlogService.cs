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

namespace Unbiased.Dashboard.Application.Services
{
    public class BlogService : IBlogService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly AwsCredentials _awsCredentials;

        public BlogService(IMediator mediator, IConfiguration configuration, IOptions<AwsCredentials> awsOptions)
        {
            _mediator = mediator;
            _configuration = configuration;
            _awsCredentials = awsOptions.Value;
        }

        public async Task<IEnumerable<BlogDto>> GetAllBlogsAsync(BlogRequestDto blogRequestDto)
        {
            try
            {
                return await _mediator.Send(new GetAllBlogsQuery(blogRequestDto));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetAllBlogsCountAsync(BlogRequestDto blogRequestDto)
        {
            try
            {
                return await _mediator.Send(new GetAllBlogsCountQuery(blogRequestDto));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BlogDto> GetBlogByIdWithImageAsync(string id)
        {
            try
            {
                return await _mediator.Send(new GetBlogByIdQuery(id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> InsertBlogAsync(InsertBlogDtoRequest blogRequestDto, int UserId, IFormFile file)
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

                blogRequestDto.Path = resultOfPicture;
                return await _mediator.Send(new InsertBlogCommand(blogRequestDto, UserId));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateBlogAsync(UpdateBlogDtoRequest blogRequestDto, IFormFile file)
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

                blogRequestDto.Path = resultOfPicture;
                return await _mediator.Send(new UpdateBlogCommand(blogRequestDto));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
