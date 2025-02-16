using MediatR;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    public class UpdateRefreshTokenByIdHandler : IRequestHandler<UpdateRefreshTokenByIdCommand, bool>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public UpdateRefreshTokenByIdHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<bool> Handle(UpdateRefreshTokenByIdCommand request, CancellationToken cancellationToken)
        {
            return await _userManagementRepository.UpdateRefreshTokenByIdAsync(request.userId, request.refreshToken,request.refreshTokenExpiration);
        }
    }
}
