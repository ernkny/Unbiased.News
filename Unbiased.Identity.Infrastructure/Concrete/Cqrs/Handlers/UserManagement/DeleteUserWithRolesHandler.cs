using MediatR;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    public class DeleteUserWithRolesHandler : IRequestHandler<DeleteUserWithRolesCommand, bool>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public DeleteUserWithRolesHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<bool> Handle(DeleteUserWithRolesCommand request, CancellationToken cancellationToken)
        {
           return await _userManagementRepository.DeleteUserWithRolesAsync(request.userId);
        }
    }
}
