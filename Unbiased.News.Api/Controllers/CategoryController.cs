using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.News.Api.Controllers
{
    /// <summary>
    /// Controller for managing categories.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="categoriesService">The categories service instance.</param>
        public CategoryController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        [HttpGet("/GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var result = await _categoriesService.GetAllCategoriesAsync();
                if (!result.Any())
                {
                    return NoContent(); 
                }

                var response = new ResponseDto<List<Category>>
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
        /// Gets all categories with article count and most visited news.
        /// </summary>
        /// <param name="language">The language code to filter categories by.</param>
        /// <returns>A list of categories with details including article counts and most visited news.</returns>
        [HttpGet("/CategoriesWithArticleCountAndMostVisitedNews")]
        public async Task<IActionResult> CategoriesWithArticleCountAndMostVisitedNews(string language)
        {
            try
            {
                var result = await _categoriesService.GetHomePageCategorieSliderWithCountAsync(language);
                var response = new ResponseDto<List<HomePageCategorieSliderWithCountDto>>
                {
                    IsSuccessful = true,
                    StatusCode = result.Any() ? 200 : 404,
                    Data = result
                };
                return result.Any() ? Ok(response) : NotFound(response); 
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
        /// Gets all categories with randomly selected generated news.
        /// </summary>
        /// <param name="language">The language code to filter categories by.</param>
        /// <returns>A list of categories with randomly selected generated news.</returns>
        [HttpGet("/CategoriesWithRandomGeneratedNews")]
        public async Task<IActionResult> CategoriesWithRandomGeneratedNews(string language)
        {
            try
            {
                var result = await _categoriesService.GetHomePageCategoriesRandomGeneratedNewsAsync(language);
                var response = new ResponseDto<List<HomePageCategoriesRandomLastGeneratedNewsDto>>
                {
                    IsSuccessful = true,
                    StatusCode = result.Any() ? 200 : 404,
                    Data = result
                };
                return result.Any() ? Ok(response) : NotFound(response);
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
        /// Gets top categories with their associated generated news.
        /// </summary>
        /// <param name="language">The language code to filter categories by.</param>
        /// <returns>A list of top categories with their generated news articles.</returns>
        [HttpGet("/TopCategoriesWithGeneratedNews")]
        public async Task<IActionResult> TopCategoriesWithGeneratedNews(string language)
        {
            try
            {
                var result = await _categoriesService.GetHomePageTopCategoriesGeneratedNewsAsync(language);
                var response = new ResponseDto<List<HomePageCategoriesRandomLastGeneratedNewsDto>>
                {
                    IsSuccessful = true,
                    StatusCode = result.Any() ? 200 : 404,
                    Data = result
                };
                return result.Any() ? Ok(response) : NotFound(response);
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
