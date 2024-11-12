using Microsoft.AspNetCore.Mvc;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Application.Interfaces.Playwright;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _insertNewsService;
        private readonly IPlaywrightScrappingService _playwrightScrapping;

        public NewsController(INewsService insertNewsService, IPlaywrightScrappingService playwrightScrapping)
        {
            _insertNewsService = insertNewsService;
            _playwrightScrapping = playwrightScrapping;
        }

        [HttpPost("InsertNews")]
        public async Task<IActionResult> InsertNews(InsertNewsDto insertNewsDto)
        {
            return Ok(await _insertNewsService.AddNewsAsync(insertNewsDto));
        }

        [HttpGet("GetNewsWithPlaywright")]
        public async Task<IActionResult> GetNewsWithPlaywright()
        {
            var result = await _playwrightScrapping.PlaywrightScrappingNewsAndAddRangeNews(); 
            return Ok(result);
        }


    }
}
