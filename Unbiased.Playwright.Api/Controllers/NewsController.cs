using Microsoft.AspNetCore.Mvc;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Application.Interfaces.Playwright;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Shared.Dtos.Concrete;

[Route("api/[controller]")]
[ApiController]
public class NewsController : ControllerBase
{
    private readonly INewsService _insertNewsService;
    private readonly IPlaywrightScrappingService _playwrightScrapping;
    private readonly INewsService _newsService;

    /// <summary>
    /// Initializes a new instance of the NewsController class.
    /// </summary>
    public NewsController(INewsService insertNewsService, IPlaywrightScrappingService playwrightScrapping, INewsService newsService)
    {
        _insertNewsService = insertNewsService;
        _playwrightScrapping = playwrightScrapping;
        _newsService = newsService;
    }

    /// <summary>
    /// Inserts new news.
    /// </summary>
    [HttpPost("InsertNews")]
    public async Task<IActionResult> InsertNews(InsertNewsDto insertNewsDto)
    {
        try
        {
            var response = new ResponseDto<Guid>();
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
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// Gets news using playwright scrapping.
    /// </summary>
    [HttpGet("GetNewsWithPlaywright")]
    public async Task<IActionResult> GetNewsWithPlaywright()
    {
        try
        {
            var response = new ResponseDto<bool>();
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
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// Generates news using AI.
    /// </summary>
    [HttpPost("GenerateNewsWithAI")]
    public async Task<IActionResult> GenerateNewsWithAI()
    {
        try
        {
            var response = new ResponseDto<bool>();
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