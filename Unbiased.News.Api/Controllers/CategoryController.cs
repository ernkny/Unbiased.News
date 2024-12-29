using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unbiased.News.Application.Interfaces;
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
                var response = new ResponseDto<List<Category>>();
                var result = await _categoriesService.GetAllCategoriesAsync();
                if (result.Count() > 0)
                {
                    response.IsSuccessful = true;
                    response.StatusCode = 200;
                    response.Data = result;
                    return Ok(response);
                }
                response.IsSuccessful = true;
                response.Data = Enumerable.Empty<Category>().ToList();
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
