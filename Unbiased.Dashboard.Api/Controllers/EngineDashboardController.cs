using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.Dashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineDashboardController : ControllerBase
    {
        private readonly IEngineService _engineService;

        public EngineDashboardController(IEngineService engineService)
        {
            _engineService = engineService;
        }


        [Authorize(Policy = "Engine Management Get")]
        [HttpGet("/GetAllEngineConfigurations")]
        public async Task<IActionResult> GetAllEngineConfigurations()
        {
            try
            {
                var result = await _engineService.GetAllEngineConfigurationsAsync();
                var response = new ResponseDto<List<EngineConfigurationDto>>
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

        [Authorize(Policy = "Engine Management Generate Content")]
        [HttpGet("/GenerateContent")]
        public async Task<IActionResult> GenerateContent([FromQuery]string url)
        {
            try
            {
               
                var result = await _engineService.GenerateContentAsync(url);
                var response = new ResponseDto<string>
                {
                    IsSuccessful = result.Any(),
                    StatusCode = result.Any() ? 200 : 204,
                    Data = result
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
    }
}
