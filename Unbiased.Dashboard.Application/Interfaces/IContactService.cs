using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Application.Interfaces
{
    /// <summary>
    /// Interface for contact service operations providing functionality for customer message management.
    /// </summary>
    public interface IContactService
    {
        /// <summary>
        /// Retrieves all customer messages with pagination support.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page for pagination.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of customer contact entities.</returns>
        Task<IEnumerable<Contact>> GetAllCustomerMessagesAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves a specific customer message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer message to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation containing the customer contact entity.</returns>
        Task<Contact> GetCustomerMessagesByIdAsync(int id);

        /// <summary>
        /// Deletes a customer message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer message to delete.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        Task<bool> DeleteCustomerMessagesByIdAsync(int id);
    }
}
