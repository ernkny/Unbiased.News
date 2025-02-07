using MediatR;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    internal class UpdateUserWithRolesHandler : IRequestHandler<UpdateUserWithRolesCommand, bool>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public UpdateUserWithRolesHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<bool> Handle(UpdateUserWithRolesCommand request, CancellationToken cancellationToken)
        {
            return await _userManagementRepository.UpdateUserWithRolesAsync(request.user);
        }
    }
}
