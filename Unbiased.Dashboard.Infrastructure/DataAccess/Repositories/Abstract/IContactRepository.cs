using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllCustomerMessagesAsync(int pageNumber, int pageSize);
        Task<Contact> GetCustomerMessagesByIdAsync(int id);
        Task<bool> DeleteCustomerMessagesByIdAsync(int id);
    }
}
