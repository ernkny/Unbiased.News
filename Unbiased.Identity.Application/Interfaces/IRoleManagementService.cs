using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Application.Interfaces
{
    public interface IRoleManagementService
    {
        Task<IEnumerable<PagesWithPermissionsDto>> GetAllPagesWithPermissionsAsync();
    }
}
