using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Represents a repository for managing news articles with images in the Unbiased Dashboard.
    /// </summary>
    public interface IBlogRepository
    {
        /// <summary>
        ///  Retrieves all blogs with pagination support based on the provided request DTO.
        /// </summary>
        /// <param name="blogRequestDto"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<BlogDto>> GetAllBlogsAsync(BlogRequestDto blogRequestDto, int pageNumber, int pageSize);

        /// <summary>
        ///  Retrieves a specific blog by its ID, including its associated image.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BlogDto> GetBlogByIdWithImageAsync(string id);

        /// <summary>
        /// Retrieves the count of all blogs based on the provided request DTO.
        /// </summary>
        /// <param name="blogRequestDto"></param>
        /// <returns></returns>
        Task<int> GetAllBlogsCountAsync(BlogRequestDto blogRequestDto);

        /// <summary>
        ///  Updates a blog using the provided DTO.
        /// </summary>
        /// <param name="blogRequestDto"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<bool> InsertBlogAsync(InsertBlogDtoRequest blogRequestDto, int UserId);

        /// <summary>
        ///  Updates a blog using the provided DTO.
        /// </summary>
        /// <param name="blogRequestDto"></param>
        /// <returns></returns>
        Task<bool> UpdateBlogAsync(UpdateBlogDtoRequest blogRequestDto);

        /// <summary>
        ///  Deletes a blog by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteBlogByIdAsync(string id);
    }
}
