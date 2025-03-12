using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.Abstract.Services
{
    public interface IContactService
    {
        Task<bool> DeleteCustomerMessageAsync(int id);
        Task<IEnumerable<Contact>> GetAllCustomerMessagesAsync(int pageNumber, int pageSize);
        Task<Contact> GetCustomerByIdAsync(int id);
    }
} 