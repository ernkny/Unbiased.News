using Microsoft.AspNetCore.Mvc;
using Microsoft.Playwright.Core;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Application.Interfaces.Playwright;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.Playwright.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _insertNewsService;
        private readonly IPlaywrightScrappingService _playwrightScrapping;
        private readonly INewsService _newsService;

        public NewsController(INewsService insertNewsService, IPlaywrightScrappingService playwrightScrapping, INewsService newsService)
        {
            _insertNewsService = insertNewsService;
            _playwrightScrapping = playwrightScrapping;
            _newsService = newsService;
        }

        [HttpPost("InsertNews")]
        public async Task<IActionResult> InsertNews(InsertNewsDto insertNewsDto)
        {
            ResponseDto<Guid> response = new ResponseDto<Guid>();
            var result = await _insertNewsService.AddNewsAsync(insertNewsDto);
            if (result.GetType() == typeof(Guid))
            {
                response.IsSuccessful = true;
                response.StatusCode = 200;
                response.Data = result;
                return Ok(response);
            }
            return Ok(response.IsSuccessful = false);
        }

        [HttpGet("GetNewsWithPlaywright")]
        public async Task<IActionResult> GetNewsWithPlaywright()
        {
            ResponseDto<bool> response = new ResponseDto<bool>();
            var result = await _playwrightScrapping.PlaywrightScrappingNewsAndAddRangeNewsAsync();
            if (result)
            {
                response.IsSuccessful = true;
                response.StatusCode = 200;
                response.Data = result;
                return Ok(response);
            }
            return Ok(response.IsSuccessful = false);
        }

        [HttpPost("GenerateNewsWithAI")]
        public async Task<IActionResult> GenerateNewsWithAI()
        {
            try
            {

                ResponseDto<bool> response = new ResponseDto<bool>();
                var result = await _newsService.SendNewsToApiForGenerateAsync();
                if (result)
                {
                    response.IsSuccessful = true;
                    response.StatusCode = 200;
                    response.Data = result;
                    return Ok(response);
                }
                return Ok(response.IsSuccessful = false);
               
               

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
