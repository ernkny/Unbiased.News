using MediatR;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.RoleManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.RoleManagement
{
    public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, bool>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public UpdateRoleHandler(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
                return await _roleManagementRepository.UpdateRoleAsync(request.role);
        }
    }
}
