using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Application.Interfaces
{
    public interface IUserManagementService
    {
        Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize);
        Task<int> GetAllUsersCountAsync();
        Task<bool> InsertUserWithRolesAsync(InsertUserWithRolesDto user);
        Task<GetUserWithRolesDto> GetUserWithRolesAsync(int userId);
    }
}
