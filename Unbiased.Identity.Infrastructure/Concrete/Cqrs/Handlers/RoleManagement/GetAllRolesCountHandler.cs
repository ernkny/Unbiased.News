using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.RoleManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.RoleManagement
{
    public class GetAllRolesCountHandler : IRequestHandler<GetAllRolesCountQuery, int>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public GetAllRolesCountHandler(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<int> Handle(GetAllRolesCountQuery request, CancellationToken cancellationToken)
        {
            return await _roleManagementRepository.GetAllRolesCountAsync();
        }
    }
}
