using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.Dashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsDashboardController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsDashboardController(INewsService newsService)
        {
            _newsService = newsService;
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
    }
}
