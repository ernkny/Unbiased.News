using Microsoft.AspNetCore.Mvc;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.News.Api.Controllers
{
    /// <summary>
    /// Controller for managing news.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsController"/> class.
        /// </summary>
        /// <param name="newsService">The news service instance.</param>
        [HttpGet("/GetAllGeneratedNews")]
        public async Task<IActionResult> GetAllGeneratedNews(string language)
        {
            try
            {
                language = string.IsNullOrEmpty(language) ? "tr" : language;
                var response = new ResponseDto<List<GeneratedNews>>();
                var result = await _newsService.GetAllGeneratedNewsAsync(language);
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

        /// <summary>
        /// Gets all generated news.
        /// </summary>
        /// <returns>A list of generated news.</returns>
        [HttpGet("/GetAllGeneratedNewsWithImage")]
        public async Task<IActionResult> GetAllGeneratedNewsWithImage(int categoryId, string language,int pageNumber = 1)
        {
            try
            {
                language=language.ToUpper();
                language=string.IsNullOrEmpty(language)?"TR":language;
                var response = new ResponseDto<List<GenerateNewsWithImageDto>>();
                var result = await _newsService.GetAllGeneratedNewsWithImageAsync(categoryId, pageNumber, language);
                if (result.Count() > 0)
                {
                    response.IsSuccessful = true;
                    response.StatusCode = 200;
                    response.Data = result.ToList();
                    return Ok(response);
                }
                response.IsSuccessful = true;
                response.Data = Enumerable.Empty<GenerateNewsWithImageDto>().ToList();
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }

        }

        /// <summary>
        /// Gets all generated news.
        /// </summary>
        /// <returns>A list of generated news.</returns>
        [HttpGet("/GetAllGeneratedNewsWithImageCount")]
        public async Task<IActionResult> GetAllGeneratedNewsWithImageCountAsync(int categoryId)
        {
             try
            {
                var response = new ResponseDto<int>();
                var result = await _newsService.GetAllGeneratedNewsWithImageCountAsync(categoryId);
                if (result > 0)
                {
                    response.IsSuccessful = true;
                    response.StatusCode = 200;
                    response.Data = result;
                    return Ok(response);
                }
                response.IsSuccessful = true;
                response.Data =0;
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
    }
}
