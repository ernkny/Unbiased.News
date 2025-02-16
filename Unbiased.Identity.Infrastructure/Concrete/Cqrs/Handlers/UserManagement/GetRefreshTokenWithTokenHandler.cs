using MediatR;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    public class GetRefreshTokenWithTokenHandler : IRequestHandler<GetRefreshTokenWithTokenQuery, User>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public GetRefreshTokenWithTokenHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<User> Handle(GetRefreshTokenWithTokenQuery request, CancellationToken cancellationToken)
        {
            return await _userManagementRepository.GetRefreshTokenWithTokenAsync(request.refreshToken);
        }
    }
}
