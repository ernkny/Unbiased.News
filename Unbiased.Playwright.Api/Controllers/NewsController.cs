using Microsoft.AspNetCore.Mvc;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IInsertNewsService _insertNewsService;

        public NewsController(IInsertNewsService insertNewsService)
        {
            _insertNewsService = insertNewsService;
        }

        [HttpPost]
        public async Task<IActionResult> InsertNews(InsertNewsDto insertNewsDto)
        {
            return Ok(await _insertNewsService.InsertNewsAsync(insertNewsDto));
        }
    }
}
