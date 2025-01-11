using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
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
        public async Task<IActionResult> GetAllGeneratedNews(string language = "tr")
        {
            try
            {
                var result = await _newsService.GetAllGeneratedNewsAsync(language.Trim().ToLower());
                var response = new ResponseDto<List<GeneratedNew>>
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

        /// <summary>
        /// Gets all generated news with images for a specific category.
        /// </summary>
        /// <param name="categoryId">The category ID.</param>
        /// <param name="language">The language of the news. Default is 'TR'.</param>
        /// <param name="pageNumber">The page number for pagination. Default is 1.</param>
        /// <returns>A list of generated news with images.</returns>
        [HttpGet("/GetAllGeneratedNewsWithImage")]
        public async Task<IActionResult> GetAllGeneratedNewsWithImage(int categoryId, string language = "TR", int pageNumber = 1)
        {
            try
            {
                language = string.IsNullOrEmpty(language) ? "TR" : language.ToUpper();

                var result = await _newsService.GetAllGeneratedNewsWithImageAsync(categoryId, pageNumber, language);
                if (!result.Any())
                {
                    return NoContent();  
                }

                var response = new ResponseDto<List<GenerateNewsWithImageDto>>
                {
                    IsSuccessful = true,
                    StatusCode = 200,
                    Data = result.ToList()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception here
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);  
            }
        }


        /// <summary>
        /// Gets the count of all generated news items with images for a specific category.
        /// </summary>
        /// <param name="categoryId">The category ID to count news items from.</param>
        /// <returns>A count of generated news items with images.</returns>
        [HttpGet("/GetAllGeneratedNewsWithImageCount")]
        public async Task<IActionResult> GetAllGeneratedNewsWithImageCountAsync(int categoryId)
        {
            try
            {
                var result = await _newsService.GetAllGeneratedNewsWithImageCountAsync(categoryId);
                var response = new ResponseDto<int>
                {
                    IsSuccessful = true,
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

        /// <summary>
        /// Retrieves a specific news item by its ID from the generated news database.
        /// This method is designed to return detailed information about a news item, including associated images,
        /// if available. It ensures a robust error handling mechanism that differentiates between not found scenarios
        /// and server errors, providing clear and actionable HTTP responses for API consumers.
        /// </summary>
        [HttpGet("/GetGeneratedNewById")]
        public async Task<IActionResult> GetGeneratedNewsById(string id)
        {
            try
            {
                var result = await _newsService.GetGeneratedNewsByIdAsync(id);
                if (result == null)
                {
                   
                    return NotFound(new ResponseDto<GenerateNewsWithImageDto>
                    {
                        IsSuccessful = false,
                        StatusCode = 404,
                        Data = null
                    });
                }

                var response = new ResponseDto<GenerateNewsWithImageDto>
                {
                    IsSuccessful = true,
                    StatusCode = 200,
                    Data = result
                };
                return Ok(response);
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
