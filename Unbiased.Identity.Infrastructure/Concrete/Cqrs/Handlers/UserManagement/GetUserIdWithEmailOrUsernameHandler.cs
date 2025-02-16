using MediatR;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    public class GetUserIdWithEmailOrUsernameHandler : IRequestHandler<GetUserIdWithEmailOrUsernameQuery, int>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public GetUserIdWithEmailOrUsernameHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<int> Handle(GetUserIdWithEmailOrUsernameQuery request, CancellationToken cancellationToken)
        {
            return await _userManagementRepository.CheckEmailOrUsernameAsync(request.EmailOrUsername);
        }
    }
}

