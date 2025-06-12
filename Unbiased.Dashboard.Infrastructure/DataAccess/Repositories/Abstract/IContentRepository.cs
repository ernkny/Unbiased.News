using Unbiased.Dashboard.Domain.Dto_s.Content;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Provides methods for managing and retrieving content subheadings, categories, and generated content.
    /// </summary>
    public interface IContentRepository
    {
        /// <summary>
        /// Retrieves all content subheadings for the dashboard with pagination and filtering options.
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
        /// Retrieves all content categories.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ContentCategories>> GetAllContentCategoriesAsync();

        /// <summary>
        ///  Retrieves the generated content by its unique identifier.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<GeneratedContentDto> GetGeneratedContentByIdAsync(int Id);
    }
}
