using MediatR;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    public class InsertUserWithRolesHandler : IRequestHandler<InsertUserWithRolesCommand, bool>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public InsertUserWithRolesHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<bool> Handle(InsertUserWithRolesCommand request, CancellationToken cancellationToken)
        {
            return await _userManagementRepository.InsertUserWithRolesAsync(request.user);
        }
    }
}
