using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.Dashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDashboardController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogDashboardController(IBlogService blogService)
        {
            _blogService = blogService;
        }


        [Authorize(Policy = "Blog Posts Get")]
        [HttpPost("/GetAllBlogs")]
        public async Task<IActionResult> GetAllBlogs(BlogRequestDto request)
        {
            try
            {
                var result = await _blogService.GetAllBlogsAsync(request);
                var response = new ResponseDto<List<BlogDto>>
                {
                    IsSuccessful = result.Any(),
                    StatusCode = result.Any() ? 200 : 204,
                    Data = result.ToList()
                };

                return result.Any() ? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = ex is not null ? ex.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        [Authorize(Policy = "Blog Posts Get")]
        [HttpPost("/GetAllBlogsCount")]
        public async Task<IActionResult> GetAllBlogsCount(BlogRequestDto request)
        {
            try
            {
                var result = await _blogService.GetAllBlogsCountAsync(request);
                var response = new ResponseDto<int>
                {
                    IsSuccessful = result > 0,
                    StatusCode = result > 0 ? 200 : 204,
                    Data = result
                };

                return result > 0 ? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = ex is not null ? ex.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        [Authorize(Policy = "Blog Posts Update")]
        [HttpGet("/GetBlogForUpdate")]
        public async Task<IActionResult> GetBlogForUpdate([FromQuery] string id)
        {
            try
            {
                var result = await _blogService.GetBlogByIdWithImageAsync(id);
                var response = new ResponseDto<BlogDto>
                {
                    IsSuccessful = result is not null,
                    StatusCode = result is not null ? 200 : 204,
                    Data = result
                };

                return result is not null ? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = ex is not null ? ex.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        [Authorize(Policy = "Blog Posts Update")]
        [HttpPost("/UpdateBlogs")]
        public async Task<IActionResult> UpdateBlogs()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var image = formCollection.Files.FirstOrDefault();
                var updateGeneratedNewsDto = JsonConvert.DeserializeObject<UpdateBlogDtoRequest>(formCollection["updateBlogDto"].FirstOrDefault());

                var result = await _blogService.UpdateBlogAsync(updateGeneratedNewsDto, image);
                var response = new ResponseDto<bool>
                {
                    IsSuccessful = result,
                    StatusCode = result ? 200 : 204,
                    Data = result
                };

                return result ? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = ex is not null ? ex.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        [Authorize(Policy = "Blog Posts Add")]
        [HttpPost("/InsertBlogs")]
        public async Task<IActionResult> InsertBlogs()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var image = formCollection.Files.FirstOrDefault();
                var insertNewsWithImageDto = JsonConvert.DeserializeObject<InsertBlogDtoRequest>(formCollection["InsertBlogWithImageDto"].FirstOrDefault());

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID is missing from the token.");
                }
                var result = await _blogService.InsertBlogAsync(insertNewsWithImageDto, Convert.ToInt32(userId), image);
                var response = new ResponseDto<bool>
                {
                    IsSuccessful = result,
                    StatusCode = result ? 200 : 204,
                    Data = result
                };

                return result ? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = ex.InnerException.Message is not null ? ex.InnerException.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        [Authorize(Policy = "Blog Posts Delete")]
        [HttpDelete("/DeleteBlogs")]
        public async Task<IActionResult> DeleteBlogs([FromQuery]string id)
        {
            try
            {
                var result= await _blogService.DeleteBlogAsync(id);
                var response = new ResponseDto<bool>
                {
                    IsSuccessful = result,
                    StatusCode = result ? 200 : 204,
                    Data = result
                };
                return result ? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = ex is not null ? ex.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}
