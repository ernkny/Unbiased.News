using MediatR;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    public class ValidateUsernameAndEmailWithRolesHandler : IRequestHandler<ValidateUsernameAndEmailWithRolesQuery, bool>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public ValidateUsernameAndEmailWithRolesHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<bool> Handle(ValidateUsernameAndEmailWithRolesQuery request, CancellationToken cancellationToken)
        {
            return await _userManagementRepository.ValidateUsernameAndEmailWithRolesAsync(request.username, request.email);
        }
    }
}
