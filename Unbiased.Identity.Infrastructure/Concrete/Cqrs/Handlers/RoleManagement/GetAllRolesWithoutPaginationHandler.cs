using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.RoleManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.RoleManagement
{
    public class GetAllRolesWithoutPaginationHandler : IRequestHandler<GetAllRolesWithoutPaginationQuery, IEnumerable<Role>>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public GetAllRolesWithoutPaginationHandler(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<IEnumerable<Role>> Handle(GetAllRolesWithoutPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _roleManagementRepository.GetAllRolessWithoutPaginationAsync();
        }
    }
}
