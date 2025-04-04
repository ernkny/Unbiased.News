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

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsController"/> class.
        /// </summary>
        /// <param name="newsService">The news service instance used for retrieving news data.</param>
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// Retrieves all generated news items with the specified language.
        /// </summary>
        /// <param name="language">The language code for filtering news. Default is 'TR'.</param>
        /// <returns>A list of all generated news items in the specified language.</returns>
        [HttpGet("/GetAllGeneratedNews")]
        public async Task<IActionResult> GetAllGeneratedNews([FromQuery]string language = "TR")
        {
            try
            {
                var result = await _newsService.GetAllGeneratedNewsAsync(language.Trim().ToUpper());
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
        /// <param name="title">Optional title filter for searching news.</param>
        /// <param name="language">The language of the news. Default is 'TR'.</param>
        /// <param name="pageNumber">The page number for pagination. Default is 1.</param>
        /// <returns>A list of generated news with images.</returns>
        [HttpGet("/GetAllGeneratedNewsWithImage")]
        public async Task<IActionResult> GetAllGeneratedNewsWithImage([FromQuery] int categoryId, string? title, string language = "TR", int pageNumber = 1)
        {
            try
            {
                language = string.IsNullOrEmpty(language) ? "TR" : language.ToUpper();

                var result = await _newsService.GetAllGeneratedNewsWithImageAsync(categoryId, pageNumber, language, title);
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
        /// <param name="title">Optional title filter for counting matching news.</param>
        /// <returns>A count of generated news items with images.</returns>
        [HttpGet("/GetAllGeneratedNewsWithImageCount")]
        public async Task<IActionResult> GetAllGeneratedNewsWithImageCountAsync(int categoryId, string? title)
        {
            try
            {
                var result = await _newsService.GetAllGeneratedNewsWithImageCountAsync(categoryId,title);
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
        /// <param name="id">The unique identifier of the news item to retrieve.</param>
        /// <returns>Detailed information about the requested news item.</returns>
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

        /// <summary>
        /// Retrieves a specific news item by its unique URL from the generated news database.
        /// This method is designed to return detailed information about a news item, including associated images,
        /// if available. It ensures a robust error handling mechanism that differentiates between not found scenarios
        /// and server errors, providing clear and actionable HTTP responses for API consumers.
        /// </summary>
        /// <param name="UniqUrl">The unique URL of the news item to retrieve.</param>
        /// <returns>Detailed information about the requested news item.</returns>
        [HttpGet("/GetGeneratedNewByUniqUrl")]
        public async Task<IActionResult> GetGeneratedNewsByUniqUrl(string UniqUrl)
        {
            try
            {
                var result = await _newsService.GetGeneratedNewsByUniqUrlAsync(UniqUrl);
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


        /// <summary>
        /// Retrieves banner news items for a specific category and language.
        /// This method returns featured news items that are suitable for display in banner sections,
        /// including associated images if available.
        /// </summary>
        /// <param name="categoryId">The category ID to retrieve banner news from.</param>
        /// <param name="language">The language of the news items to retrieve.</param>
        /// <returns>A list of banner news items with images.</returns>
        [HttpGet("/GetBannerGeneratedNews")]
        public async Task<IActionResult> GetBannerGeneratedNews([FromQuery]int categoryId, string language)
        {
            try
            {
                var result = await _newsService.GetBannerGeneratedNewsWithImageAsync(categoryId, language);
                if (result == null)
                {

                    return NotFound(new ResponseDto<List<GenerateNewsWithImageDto>>
                    {
                        IsSuccessful = false,
                        StatusCode = 404,
                        Data = null
                    });
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
        /// Retrieves the top latest news items from a specific category for the detail page,
        /// excluding the news item with the provided ID.
        /// </summary>
        /// <param name="categoryId">The category ID to retrieve news from.</param>
        /// <param name="id">The ID of the news item to exclude from results.</param>
        /// <param name="language">The language of the news items to retrieve.</param>
        /// <returns>A list of the latest top news items from the specified category.</returns>
        [HttpGet("/GetAllLastTopGeneratedNewsWithCategoryIdForDetailPage")]
        public async Task<IActionResult> GetAllLastTopGeneratedNewsWithCategoryIdForDetailPage(int categoryId, string id, string language)
        {
            try
            {
                var result = await _newsService.GetAllLastTopGeneratedNewsWithCategoryIdForDetailAsync(categoryId, id, language);
                var response = new ResponseDto<List<GenerateNewsWithImageDto>>
                {
                    IsSuccessful = true,
                    StatusCode = result.Count() > 0 ? 200 : 204,
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
                    Data = "An error occurred while processing your request"
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Retrieves questions and answers related to a specific match.
        /// </summary>
        /// <param name="MatchId">The unique identifier of the match to retrieve Q&A for.</param>
        /// <returns>A collection of questions and answers for the specified match.</returns>
        [HttpGet("/GetQuestionsAndAnswers")]
        public async Task<IActionResult> GetQuestionsAndAnswers([FromQuery]string MatchId)
        {
            try
            {
                var result = await _newsService.GetAllQuestionsAndAnswerWithMatchIdAsync(MatchId);
                var response = new ResponseDto<IEnumerable<QuestionAnswerDto>>
                {
                    IsSuccessful = true,
                    StatusCode = result.Count() > 0 ? 200 : 204,
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

        /// <summary>
        /// Retrieves statistical information about all news items in the system.
        /// </summary>
        /// <returns>Statistical data about news items, such as total counts, categories, and more.</returns>
        [HttpGet("/GetAllNewsStatistics")]
        public async Task<IActionResult> GetAllNewsStatistics()
        {
            try
            {
                var result = await _newsService.GetAllStatisticsAsync();
                var response = new ResponseDto<StatisticsDto>
                {
                    IsSuccessful = true,
                    StatusCode = result is not null  ? 200 : 204,
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
    }
}
