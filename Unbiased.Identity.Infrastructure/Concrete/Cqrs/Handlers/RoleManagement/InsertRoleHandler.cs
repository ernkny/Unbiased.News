using MediatR;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.RoleManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.RoleManagement
{
    public class InsertRoleHandler : IRequestHandler<InsertRoleCommand, bool>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public InsertRoleHandler(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<bool> Handle(InsertRoleCommand request, CancellationToken cancellationToken)
        {
           return await _roleManagementRepository.InsertRoleAsync(request.role);
        }
    }
}
