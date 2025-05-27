using Microsoft.AspNetCore.Http;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Application.Interfaces
{
    /// <summary>
    /// Interface for blog service operations providing CRUD functionality for blog management.
    /// </summary>
    public interface IBlogService
    {
        /// <summary>
        /// Retrieves all blogs based on the specified request parameters with pagination and filtering.
        /// </summary>
        /// <param name="blogRequestDto">The request DTO containing pagination and filtering parameters.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of blog DTOs.</returns>
        Task<IEnumerable<BlogDto>> GetAllBlogsAsync(BlogRequestDto blogRequestDto);

        /// <summary>
        /// Retrieves a specific blog by its unique identifier including associated image data.
        /// </summary>
        /// <param name="id">The unique identifier of the blog to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation containing the blog DTO with image.</returns>
        Task<BlogDto> GetBlogByIdWithImageAsync(string id);

        /// <summary>
        /// Gets the total count of blogs based on the specified filtering criteria.
        /// </summary>
        /// <param name="blogRequestDto">The request DTO containing filtering parameters.</param>
        /// <returns>A task that represents the asynchronous operation containing the total count of blogs.</returns>
        Task<int> GetAllBlogsCountAsync(BlogRequestDto blogRequestDto);

        /// <summary>
        /// Creates a new blog with the specified data and associated image file.
        /// </summary>
        /// <param name="blogRequestDto">The blog data transfer object containing the blog information.</param>
        /// <param name="UserId">The unique identifier of the user creating the blog.</param>
        /// <param name="file">The image file to associate with the blog.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> InsertBlogAsync(InsertBlogDtoRequest blogRequestDto, int UserId, IFormFile file);

        /// <summary>
        /// Updates an existing blog with new data and optionally a new image file.
        /// </summary>
        /// <param name="blogRequestDto">The blog data transfer object containing updated information.</param>
        /// <param name="file">The new image file to associate with the blog (optional).</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> UpdateBlogAsync(UpdateBlogDtoRequest blogRequestDto, IFormFile file);

        /// <summary>
        /// Deletes a blog by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the blog to delete.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> DeleteBlogAsync(string id);
    }
}
