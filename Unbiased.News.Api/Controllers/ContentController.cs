using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;
using static MassTransit.ValidationResultExtensions;

namespace Unbiased.News.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly IMemoryCache _memoryCache;

        public ContentController(IContentService contentService, IMemoryCache memoryCache)
        {
            _contentService = contentService;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentController"/> class.
        /// </summary>
        /// <param name="contentService">The content Service instance.</param>
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
        /// Initializes a new instance of the <see cref="ContentController"/> class.
        /// </summary>
        /// <param name="contentService">The content Service instance.</param>
        [HttpGet("/GetAllDailyHoroscopeWithId")]
        public async Task<IActionResult> GetAllDailyHoroscopeWithId(int id)
        {
            try
            {
                const string cacheKey = "DailyLastHoroscopes";
                List<HoroscopeDailyDetail> cachedResult;
                if (!_memoryCache.TryGetValue(cacheKey, out cachedResult))
                {
                    var result =  await _contentService.GetDailyLastHoroscopesAsync();
                    cachedResult=result.ToList();
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
        /// Initializes a new instance of the <see cref="ContentController"/> class.
        /// </summary>
        /// <param name="contentService">The content Service instance.</param>
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
                    Data =  result is not null ? result : throw new ArgumentNullException(nameof(result))
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
