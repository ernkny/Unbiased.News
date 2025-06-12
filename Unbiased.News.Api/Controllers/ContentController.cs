using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.News.Api.Controllers
{
    /// <summary>
    /// Controller for managing content operations like horoscopes and daily content.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentController"/> class.
        /// </summary>
        /// <param name="contentService">The content service instance for accessing content data.</param>
        /// <param name="memoryCache">The memory cache instance for caching content data.</param>
        public ContentController(IContentService contentService, IMemoryCache memoryCache)
        {
            _contentService = contentService;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Retrieves all daily horoscope information.
        /// </summary>
        /// <returns>A list of daily horoscope details for all zodiac signs.</returns>
        [HttpGet("/GetAllDailyHoroscope")]
        public async Task<IActionResult> GetAllDailyHoroscope()
        {
            try
            {
                var result = await _contentService.GetDailyLastHoroscopesAsync();
                var response = new ResponseDto<List<HoroscopeDailyDetail>>
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
        /// Retrieves daily horoscope information for a specific zodiac sign by ID.
        /// Uses caching to optimize performance for frequently accessed data.
        /// </summary>
        /// <param name="id">The ID of the horoscope (zodiac sign) to retrieve.</param>
        /// <returns>Daily horoscope details for the specified zodiac sign.</returns>
        [HttpGet("/GetAllDailyHoroscopeWithId")]
        public async Task<IActionResult> GetAllDailyHoroscopeWithId([FromQuery] int id)
        {
            try
            {
                const string cacheKey = "DailyLastHoroscopes";
                List<HoroscopeDailyDetail> cachedResult;
                if (!_memoryCache.TryGetValue(cacheKey, out cachedResult))
                {
                    var result = await _contentService.GetDailyLastHoroscopesAsync();
                    cachedResult = result.ToList();
                    if (cachedResult.Any())
                    {
                        _memoryCache.Set(cacheKey, cachedResult, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3)
                        });
                    }
                    else
                    {
                        return NoContent();
                    }
                }

                var filteredResult = cachedResult.FirstOrDefault(h => h.HoroscopeId == id);

                if (filteredResult is null)
                {
                    return NoContent();
                }

                var response = new ResponseDto<HoroscopeDailyDetail>
                {
                    IsSuccessful = true,
                    StatusCode = 200,
                    Data = filteredResult
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
        /// Retrieves the latest daily content information.
        /// Requires authentication to access this endpoint.
        /// </summary>
        /// <returns>The latest content information.</returns>
        [HttpGet("/GetDailyContent")]
        public async Task<IActionResult> GetDailyContent()
        {
            try
            {
                var result = await _contentService.GetLastContentAsync();
                var response = new ResponseDto<Contents>
                {
                    IsSuccessful = result is not null,
                    StatusCode = result is not null ? 200 : 204,
                    Data = result is not null ? result : throw new ArgumentNullException(nameof(result))
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
        /// <summary>
        /// Retrieves the latest daily content information.
        /// Requires authentication to access this endpoint.
        /// </summary>
        /// <returns>The latest content information.</returns>
        [HttpGet("/GetAllContentForHomePage")]
        public async Task<IActionResult> GetAllContentForHomePage([FromQuery]string language)
        {
            try
            {
                var result = await _contentService.GetAllContentWithImageForHomePageAsync(language);
                var response = new ResponseDto<IEnumerable<ContentSubHeadingWithImageDto>>
                {
                    IsSuccessful = result is not null,
                    StatusCode = result is not null ? 200 : 204,
                    Data = result is not null ? result : throw new ArgumentNullException(nameof(result))
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


        /// <summary>
        /// Retrieves subheadings for a specific category ID with pagination.
        /// </summary>
        /// <param name="id">The category ID</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <returns>A paginated list of subheadings for the specified category</returns>
        [HttpGet("/GetSubheadingsWithCategoryId")]
        public async Task<IActionResult> GetSubheadingsWithCategoryId([FromQuery] int id, int pageNumber = 1)
        {
            try
            {
                var result = await _contentService.ContentSubHeadingsAsync(id, pageNumber);
                var response = new ResponseDto<IEnumerable<ContentSubHeading>>
                {
                    IsSuccessful = result.Any(),
                    StatusCode = result.Any() ? 200 : 204,
                    Data = result
                };
                return result is not null ? Ok(response) : NoContent();
            }
            catch (Exception)
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
        /// Gets the total count of subheadings for a specific category ID.
        /// </summary>
        /// <param name="id">The category ID</param>
        /// <returns>The total count of subheadings for the specified category</returns>
        [HttpGet("/GetSubheadingsCountWithCategoryId")]
        public async Task<IActionResult> GetSubheadingsCountWithCategoryId([FromQuery] int id)
        {
            try
            {
                var result = await _contentService.ContentSubHeadingsCountAsync(id);
                var response = new ResponseDto<int>
                {
                    IsSuccessful = true,
                    StatusCode = result > 0 ? 200 : 204,
                    Data = result
                };
                return result > 0 ? Ok(response) : NoContent();
            }
            catch (Exception)
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
        /// Retrieves content details by its unique URL.
        /// </summary>
        /// <param name="uniqUrl">The unique URL of the content</param>
        /// <returns>The details of the requested content</returns>
        [HttpGet("/GetContentDetail")]
        public async Task<IActionResult> GetContentDetail([FromQuery] string uniqUrl)
        {
            try
            {
                var result = await _contentService.GetGeneratedContentByUrlAsync(uniqUrl);
                var response = new ResponseDto<GeneratedContentDto>
                {
                    IsSuccessful = true,
                    StatusCode = result is not null ? 200 : 204,
                    Data = result
                };
                return result is not null ? Ok(response) : NoContent();
            }
            catch (Exception)
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
