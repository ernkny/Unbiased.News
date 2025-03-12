using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Application.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllCustomerMessagesAsync(int pageNumber, int pageSize);
        Task<Contact> GetCustomerMessagesByIdAsync(int id);
        Task<bool> DeleteCustomerMessagesByIdAsync(int id);
    }
}
