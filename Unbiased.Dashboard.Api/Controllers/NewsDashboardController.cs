using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Common.Abstract.Helpers;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.Dashboard.Api.Controllers
{
    /// <summary>
    /// Controller for managing news-related operations in the dashboard.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NewsDashboardController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly ISocialMediaImageGenerator _socialMediaImageGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsDashboardController"/> class.
        /// </summary>
        /// <param name="newsService"></param>
        /// <param name="categoryService"></param>
        public NewsDashboardController(INewsService newsService, ICategoryService categoryService, IWebHostEnvironment env, ISocialMediaImageGenerator socialMediaImageGenerator)
        {
            _newsService = newsService;
            _categoryService = categoryService;
            _env = env;
            _socialMediaImageGenerator = socialMediaImageGenerator;
        }

        /// <summary>
        ///  Retrieves all categories from the system.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "News Get")]
        [HttpGet("/GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var result = await _categoryService.GetAllCategoriesAsync();
                var response = new ResponseDto<List<Category>>
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
                    Data = ex.InnerException.Message is not null ? ex.InnerException.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        ///   Retrieves all generated news items with an image based on the provided request parameters.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy ="News Get")]
        [HttpPost("/GetAllGeneratedNews")]
        public async Task<IActionResult> GetAllGeneratedNews(GetGeneratedNewsWithImagePathRequestDto request)
        {
            try
            {
                var result = await _newsService.GetAllGenerateNewsWithImageAsync(request);
                var response = new ResponseDto<List<GenerateNewsWithImageDto>>
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
                    Data = ex.InnerException.Message is not null ? ex.InnerException.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        ///  Retrieves the count of all generated news items with an image based on the provided request parameters.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "News Get")]
        [HttpPost("/GetAllGeneratedNewsCount")]
        public async Task<IActionResult> GetAllGeneratedNewsCount(GetGeneratedNewsWithImagePathRequestDto request)
        {
            try
            {
                var result = await _newsService.GetAllGenerateNewsWithImageCountAsync(request);
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
                    Data = ex.InnerException.Message is not null ? ex.InnerException.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        ///   Retrieves a specific generated news item with an image based on the provided ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "News Update")]
        [HttpGet("/GetAllGeneratedNewsForUpdate")]
        public async Task<IActionResult> GetAllGeneratedNewsForUpdate([FromQuery]string id)
        {
            try
            {
                var result = await _newsService.GetGeneratedNewsByIdWithImageAsync(id);
                var response = new ResponseDto<GenerateNewsWithImageDto>
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
                    Data = ex.InnerException.Message is not null ? ex.InnerException.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        ///  Updates an existing news item with an image based on the provided data.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "News Update")]
        [HttpPost("/UpdateGeneratedNews")]
        public async Task<IActionResult> UpdateGeneratedNews()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var image = formCollection.Files.FirstOrDefault();
                var updateGeneratedNewsDto = JsonConvert.DeserializeObject<UpdateGeneratedNewsDto>(formCollection["updateGeneratedNewsDto"].FirstOrDefault());

                var result = await _newsService.UpdateGeneratedNewsWithImageAsync(image, updateGeneratedNewsDto);
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

        /// <summary>
        ///  Inserts a new news item with an image based on the provided data.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "News Add")]
        [HttpPost("/InsertGeneratedNews")]
        public async Task<IActionResult> InsertGeneratedNews()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var image = formCollection.Files.FirstOrDefault();
                var insertNewsWithImageDto = JsonConvert.DeserializeObject<InsertNewsWithImageDto>(formCollection["InsertNewsWithImageDto"].FirstOrDefault());

                var result = await _newsService.InsertNewsWithImageAsync(image, insertNewsWithImageDto);
                var response = new ResponseDto<bool>
                {
                    IsSuccessful = result,
                    StatusCode = result ? 200 : 204,
                    Data = result
                };
                return response is not null ? Ok(response) : NoContent();
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

        /// <summary>
        ///  Deletes a news item based on the provided ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "News Delete")]
        [HttpDelete("/DeleteNews")]
        public async Task<IActionResult> DeleteNews([FromQuery] string id)
        {
            try
            {
                var result = await _newsService.DeleteNewsAsync(id);
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

        [HttpPost("/generate-from-url")]
        public async Task<IActionResult> GenerateFromUrl([FromQuery] string imageUrl, [FromQuery] string title)
        {
            var imageBytes = await _socialMediaImageGenerator.GenerateFromUrlAsync(imageUrl, title, GetLogoPath());
            var base64 = Convert.ToBase64String(imageBytes);
            return Ok(new { ImageBase64 = base64 });
        }
        private string GetLogoPath()
        {
            var image= Path.Combine(_env.WebRootPath, "Images", "logo.png");
            return image;
        }
    }
}
