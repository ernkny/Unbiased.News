using MediatR;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    public class GetUserWithRolesHandler : IRequestHandler<GetUserWithRolesQuery, GetUserWithRolesDto>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public GetUserWithRolesHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<GetUserWithRolesDto> Handle(GetUserWithRolesQuery request, CancellationToken cancellationToken)
        {
            return await _userManagementRepository.GetUserWithRolesAsync(request.userId);
        }
    }
}
