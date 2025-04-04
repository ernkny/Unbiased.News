using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.News.Api.Controllers
{
    /// <summary>
    /// Controller for managing blog operations and retrieving blog content.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogController"/> class.
        /// </summary>
        /// <param name="blogService">The blog service instance for accessing blog data.</param>
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        /// <summary>
        /// Retrieves all blog posts with associated images, supporting pagination and filtering.
        /// </summary>
        /// <param name="language">The language code to filter blogs by.</param>
        /// <param name="searchData">Optional search term to filter blog posts.</param>
        /// <param name="pageNumber">The page number for pagination. Default is 1.</param>
        /// <returns>A list of blog posts with their associated images.</returns>
        [HttpGet("/GetAllBlogsWithImage")]
        public async Task<IActionResult> GetAllBlogsWithImage(string language,string? searchData,int pageNumber = 1)
        {
            try
            {

                var result = await _blogService.GetAllBlogsWithImageAsync(language, pageNumber,searchData);
                if (!result.Any())
                {
                    return NoContent();
                }

                var response = new ResponseDto<List<BlogWithImageDto>>
                {
                    IsSuccessful = true,
                    StatusCode = 200,
                    Data = result.ToList()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Gets the total count of blog posts matching the specified language and search criteria.
        /// </summary>
        /// <param name="language">The language code to filter blogs by.</param>
        /// <param name="searchData">Optional search term to filter blog posts.</param>
        /// <returns>The total count of matching blog posts.</returns>
        [HttpGet("/GetAllBlogsWithCount")]
        public async Task<IActionResult> GetAllBlogsWithCount(string language,string? searchData)
        {
            try
            {

                var result = await _blogService.GetAllBlogsWithImageCountAsync(language,searchData);
                if (result<=0)
                {
                    return NoContent();
                }

                var response = new ResponseDto<int>
                {
                    IsSuccessful = true,
                    StatusCode = 200,
                    Data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Retrieves a specific blog post by its unique URL.
        /// </summary>
        /// <param name="uniqUrl">The unique URL of the blog post to retrieve.</param>
        /// <returns>The requested blog post with its associated image.</returns>
        [HttpGet("/GetBlogWithImageByUniqUrl")]
        public async Task<IActionResult> GetBlogWithImageByUniqUrl(string uniqUrl)
        {
            try
            {

                var result = await _blogService.GetBlogWithImageByUniqUrlAsync(uniqUrl);
                if (result is null)
                {
                    return NoContent();
                }

                var response = new ResponseDto<BlogWithImageDto>
                {
                    IsSuccessful = true,
                    StatusCode = 200,
                    Data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}
