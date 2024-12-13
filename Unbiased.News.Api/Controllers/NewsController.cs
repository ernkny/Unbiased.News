using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.News.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("/GetAllGeneratedNews")]
        public async Task<IActionResult> GetAllGeneratedNews()
        {
            try
            {
                var response = new ResponseDto<List<GeneratedNews>>();
                var result = await _newsService.GetAllGeneratedNewsAsync();
                if (result.Count()>0)
                {
                    response.IsSuccessful = true;
                    response.StatusCode = 200;
                    response.Data = result.ToList();
                    return Ok(response);
                }
                return Ok(response.IsSuccessful = false);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        [HttpGet("/GetAllGeneratedNewsWithImage")]
        public async Task<IActionResult> GetAllGeneratedNewsWithImage()
        {
            try
            {
                var response = new ResponseDto<List<GenerateNewsWithImageDto>>();
                var result = await _newsService.GetAllGeneratedNewsWithImageAsync();
                if (result.Count() > 0)
                {
                    response.IsSuccessful = true;
                    response.StatusCode = 200;
                    response.Data = result.ToList();
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
