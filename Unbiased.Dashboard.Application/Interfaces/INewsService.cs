using Microsoft.AspNetCore.Http;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Application.Interfaces
{
    /// <summary>
    /// Interface for news service operations providing CRUD functionality for generated news management with image support.
    /// </summary>
    public interface INewsService
    {
        /// <summary>
        /// Retrieves all generated news with images based on the specified request parameters.
        /// </summary>
        /// <param name="requestDto">The request DTO containing parameters for retrieving generated news with images.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of generated news with image DTOs.</returns>
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGenerateNewsWithImageAsync(GetGeneratedNewsWithImagePathRequestDto requestDto);

        /// <summary>
        /// Gets the total count of generated news with images based on the specified request parameters.
        /// </summary>
        /// <param name="requestDto">The request DTO containing parameters for counting generated news with images.</param>
        /// <returns>A task that represents the asynchronous operation containing the total count of generated news with images.</returns>
        Task<int> GetAllGenerateNewsWithImageCountAsync(GetGeneratedNewsWithImagePathRequestDto requestDto);

        /// <summary>
        /// Retrieves a specific generated news item with image by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the generated news item to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation containing the generated news with image DTO.</returns>
        Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdWithImageAsync(string id);

        /// <summary>
        /// Updates an existing generated news item with new data and optionally a new image file.
        /// </summary>
        /// <param name="file">The new image file to associate with the news item (optional).</param>
        /// <param name="generateNewsWithImageDto">The news data transfer object containing updated information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> UpdateGeneratedNewsWithImageAsync(IFormFile file, UpdateGeneratedNewsDto generateNewsWithImageDto);

        /// <summary>
        /// Creates a new news item with the specified data and associated image file.
        /// </summary>
        /// <param name="file">The image file to associate with the news item.</param>
        /// <param name="insertNewsWithImageDto">The news data transfer object containing the news information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> InsertNewsWithImageAsync(IFormFile file, InsertNewsWithImageDto insertNewsWithImageDto);

        /// <summary>
        /// Deletes a news item by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the news item to delete.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> DeleteNewsAsync(string id);
    }
}
