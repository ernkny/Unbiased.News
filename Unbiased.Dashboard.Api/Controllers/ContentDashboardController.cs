using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Domain.Dto_s.Content;
using Unbiased.Shared.Dtos.Concrete;
using static System.Net.Mime.MediaTypeNames;

namespace Unbiased.Dashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentDashboardController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly IValidator<UpdateAllContentDataRequest> _validator;

        /// <summary>
        ///  Initializes a new instance of the <see cref="ContentDashboardController"/> class.
        /// </summary>
        /// <param name="contentService"></param>
        public ContentDashboardController(IContentService contentService, IValidator<UpdateAllContentDataRequest> validator)
        {
            _contentService = contentService;
            _validator = validator;
        }

        /// <summary>
        ///  Retrieves all contents based on the specified parameters such as page number, page size, language, category ID, and processing status.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="language"></param>
        /// <param name="categoryId"></param>
        /// <param name="IsProcessed"></param>
        /// <returns></returns>
        [Authorize(Policy = "Content Management Get")]
        [HttpGet("/GetAllContents")]
        public async Task<IActionResult> GetAllContents(int pageNumber, int pageSize, string language, int? categoryId,bool? IsProcessed)
        {
            try
            {

                var result = await _contentService.GetAllContentsAsync(pageNumber, pageSize, language, categoryId, IsProcessed);
                var response = new ResponseDto<List<ContentSubheadingDto>>
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
                    Data = ex.InnerException.Message is not null ? ex.InnerException.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        ///  Retrieves the count of all contents based on the specified language, category ID, and processing status.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="categoryId"></param>
        /// <param name="IsProcessed"></param>
        /// <returns></returns>
        [Authorize(Policy = "Content Management Get")]
        [HttpGet("/GetAllContentsCount")]
        public async Task<IActionResult> GetAlGetAllContentsCountlContents(string language, int? categoryId, bool? IsProcessed)
        {
            try
            {

                var result = await _contentService.GetAllContentsCountAsync(language, categoryId, IsProcessed);
                var response = new ResponseDto<int>
                {
                    IsSuccessful = result > 0,
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
                    Data = ex.InnerException.Message is not null ? ex.InnerException.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        ///  Retrieves all content categories.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "Content Management Get")]
        [HttpGet("/GetAllContentCategories")]
        public async Task<IActionResult> GetAllContentCategories()
        {
            try
            {

                var result = await _contentService.GetAllContentCategoriesAsync();
                var response = new ResponseDto<IEnumerable<ContentCategories>>
                {
                    IsSuccessful = result.Any(),
                    StatusCode = result.Any() ? 200 : 204,
                    Data = result
                };

                return result.Any()? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = ex.InnerException.Message is not null ? ex.InnerException.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        ///  Retrieves content by its ID.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize(Policy = "Content Management Get")]
        [HttpGet("/GetContentWithId")]
        public async Task<IActionResult> GetContentWithId(int Id)
        {
            try
            {

                var result = await _contentService.GetGeneratedContentByIdAsync(Id);
                var response = new ResponseDto<GeneratedContentDto>
                {
                    IsSuccessful = result is not null,
                    StatusCode = result is not null ? 200 : 204,
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
                    Data = ex.InnerException.Message is not null ? ex.InnerException.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Updates the content with the provided data and image. 
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "Content Management Update")]
        [HttpPost("/UpdateContent")]
        public async Task<IActionResult> UpdateContent()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var image = formCollection.Files.FirstOrDefault();
                var contentDto = JsonConvert.DeserializeObject<UpdateAllContentDataRequest>(formCollection["UpdateAllContentDataRequest"].FirstOrDefault());
                if (contentDto is null)
                {
                    return BadRequest(contentDto);
                }

                var validationResult = await _validator.ValidateAsync(contentDto);
                if (!validationResult.IsValid)
                {
                    var response = new
                    {
                        IsSuccessful = false,
                        StatusCode = 400,
                        Errors = validationResult.Errors.Select(x => new
                        {
                            Field = x.PropertyName,
                            Message = x.ErrorMessage
                        }).ToList()
                    };
                    return BadRequest(response);
                }

                if (image is not null)
                {
                    contentDto.ImagePath= await _contentService.UpdateGenereatedContentImageAsync(image);
                }

                var result= _contentService.UpdateGenerateContentAsync(contentDto);
                return Ok(new ResponseDto<bool>
                {
                    IsSuccessful = true,
                    StatusCode = 200,
                    Data = true
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = ex.InnerException.Message is not null ? ex.InnerException.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}