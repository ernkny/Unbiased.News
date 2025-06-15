using Microsoft.AspNetCore.Http;
using Unbiased.Dashboard.Domain.Dto_s.Content;

namespace Unbiased.Dashboard.Application.Interfaces
{
    /// <summary>
    ///  Provides methods for managing and retrieving content-related data.
    /// </summary>
    public interface IContentService
    {
        /// <summary>
        ///  Retrieves all content subheadings based on the specified parameters such as page number, page size, language, category ID, and processing status.
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="Language"></param>
        /// <param name="CategoryId"></param>
        /// <param name="IsProcess"></param>
        /// <returns></returns>
        Task<IEnumerable<ContentSubheadingDto>> GetAllContentsAsync(int PageNumber, int PageSize, string Language, int? CategoryId, bool? IsProcess);

        /// <summary>
        ///  Retrieves the total count of all content based on the specified language, category ID, and processing status.
        /// </summary>
        /// <param name="Language"></param>
        /// <param name="CategoryId"></param>
        /// <param name="IsProcess"></param>
        /// <returns></returns>
        Task<int> GetAllContentsCountAsync(string Language, int? CategoryId, bool? IsProcess);

        /// <summary>
        ///   Retrieves all content categories.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ContentCategories>> GetAllContentCategoriesAsync();

        /// <summary>
        ///  Retrieves the generated content by its unique identifier.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<GeneratedContentDto> GetGeneratedContentByIdAsync(int Id);

        /// <summary>
        ///  Updates the generated content based on the provided request parameters.
        /// </summary>
        /// <param name="updateAllContentDataRequest"></param>
        /// <returns></returns>
        Task<bool> UpdateGenerateContentAsync(UpdateAllContentDataRequest updateAllContentDataRequest);

        /// <summary>
        ///   Updates the generated content image based on the provided file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<string> UpdateGenereatedContentImageAsync(IFormFile file);
    }
}
