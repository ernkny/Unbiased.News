using MediatR;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.RoleManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.RoleManagement
{
    public class GetRoleByIdHandlers : IRequestHandler<GetRoleByIdQuery, RoleGetByIdDto>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public GetRoleByIdHandlers(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<RoleGetByIdDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _roleManagementRepository.GetRoleById(request.roleId);
        }
    }
}
