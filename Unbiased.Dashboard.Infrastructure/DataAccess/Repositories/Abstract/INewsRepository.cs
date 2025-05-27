using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    ///  Represents a repository for managing news articles with images in the Unbiased Dashboard.
    /// </summary>
    public interface INewsRepository
    {
        /// <summary>
        /// Retrieves all generated news articles with images based on the provided request DTO.
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync(GetGeneratedNewsWithImagePathRequestDto requestDto);

        /// <summary>
        /// Retrieves the count of all generated news articles with images based on the provided request DTO.
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> GetAllGeneratedNewsWithImageCountAsync(GetGeneratedNewsWithImagePathRequestDto requestDto);

        /// <summary>
        ///  Retrieves a specific generated news article with image by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdWithImageAsync(string id);

        /// <summary>
        /// Updates a generated news article with image using the provided DTO.
        /// </summary>
        /// <param name="generatedNewsDto"></param>
        /// <returns></returns>
        Task<bool> UpdateGeneratedNewsWithImageAsync(UpdateGeneratedNewsDto generatedNewsDto);

        /// <summary>
        ///  Inserts a new generated news article with image using the provided DTO.
        /// </summary>
        /// <param name="insertNewsWithImageDto"></param>
        /// <returns></returns>
        Task<bool> InsertGeneratedNewsWithImageAsync(InsertNewsWithImageDto insertNewsWithImageDto);

        /// <summary>
        ///  Deletes a generated news article with image by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteNewsByIdAsync(string id);

    }
}
