using MediatR;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    public class ValidateUserWithRolesHandller : IRequestHandler<ValidateUserWithRolesQuery, bool>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public ValidateUserWithRolesHandller(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<bool> Handle(ValidateUserWithRolesQuery request, CancellationToken cancellationToken)
        {
            return await _userManagementRepository.ValidateUserWithRolesAsync(request.user);
        }
    }
}
