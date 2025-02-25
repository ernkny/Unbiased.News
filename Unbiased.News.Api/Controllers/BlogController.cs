using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.News.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("/GetAllBlogsWithImage")]
        public async Task<IActionResult> GetAllBlogsWithImage(string? searchData,int pageNumber = 1)
        {
            try
            {

                var result = await _blogService.GetAllBlogsWithImageAsync(pageNumber,searchData);
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

        [HttpGet("/GetAllBlogsWithCount")]
        public async Task<IActionResult> GetAllBlogsWithCount(string? searchData)
        {
            try
            {

                var result = await _blogService.GetAllBlogsWithImageCountAsync(searchData);
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
