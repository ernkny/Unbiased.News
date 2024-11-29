using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// The interface for the News Image Repository.
    /// This interface provides methods to add a news image and get news without images.
    /// </summary>
    public interface INewsImageRepository
    {
        /// <summary>
        /// Adds a news image to the repository.
        /// This method takes an instance of InsertNewsImageDto and returns a Task with a Guid representing the ID of the newly added image.
        /// </summary>
        /// <param name="addNewsImageDto">The DTO containing the news image data to be added.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the ID of the newly added image.</returns>
        Task<Guid> AddNewsImageAsync(InsertNewsImageDto addNewsImageDto);

        /// <summary>
        /// Retrieves news items from the repository that do not have an associated image.
        /// This method takes a DateTime representing the start date and returns a Task with an IEnumerable of GetNewsWithoutImageDto.
        /// </summary>
        /// <param name="startDate">The start date for filtering news items.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains an IEnumerable of GetNewsWithoutImageDto.</returns>
        Task<IEnumerable<GetNewsWithoutImageDto>> GetNewsWithoutImages(DateTime startDate);
    }
}
