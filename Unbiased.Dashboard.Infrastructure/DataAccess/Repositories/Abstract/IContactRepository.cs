using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract
{
    /// <summary>
    /// Represents a repository for managing customer contact messages in the Unbiased Dashboard.
    /// </summary>
    public interface IContactRepository
    {
        /// <summary>
        ///  Retrieves all customer messages with pagination support.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<Contact>> GetAllCustomerMessagesAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves the count of all customer messages.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Contact> GetCustomerMessagesByIdAsync(int id);

        /// <summary>
        /// Updates a customer message using the provided DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteCustomerMessagesByIdAsync(int id);
    }
}