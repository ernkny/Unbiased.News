using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface IUserManagementRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize);
        Task<int> GetAllUsersCountAsync();
        Task<bool> InsertUserWithRolesAsync(InsertUserWithRolesDto user);
        Task<bool> ValidateUserWithRolesAsync(InsertUserWithRolesDto user);
    }
}
