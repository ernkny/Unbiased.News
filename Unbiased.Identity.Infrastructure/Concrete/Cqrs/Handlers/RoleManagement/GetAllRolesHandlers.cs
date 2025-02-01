using MediatR;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.RoleManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.RoleManagement
{
    public class GetAllRolesHandlers : IRequestHandler<GetAllRolesQuery, IEnumerable<Role>>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public GetAllRolesHandlers(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<IEnumerable<Role>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            return await _roleManagementRepository.GetAllRolesAsync(request.pageNumber, request.pageSize);
        }
    }
}
