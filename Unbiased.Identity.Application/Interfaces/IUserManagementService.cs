using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Application.Interfaces
{
    public interface IUserManagementService
    {
        Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize);
        Task<int> GetAllUsersCountAsync();
    }
}
