using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.Dashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsDashboardController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;

        public NewsDashboardController(INewsService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }


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
                    Data = "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }
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
                    Data = "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

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
                    Data = "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

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
                    Data = "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

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
                    Data = "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

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

                return result ? Ok(response) : NoContent();
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
