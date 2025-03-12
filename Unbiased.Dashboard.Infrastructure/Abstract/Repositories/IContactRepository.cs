using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.Abstract.Repositories
{
    public interface IContactRepository
    {
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Contact>> GetAllAsync(int pageNumber, int pageSize);
        Task<Contact> GetByIdAsync(int id);
    }
} 