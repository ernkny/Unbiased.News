using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface IIdentityRepository
    {
        Task<bool> InsertRoleWithPermissions(RoleWithPermissionDto roleWithPermissionDto);
    }
}
