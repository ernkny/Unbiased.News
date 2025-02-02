using MediatR;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    public class GetAllUsersCountHandler : IRequestHandler<GetAllUsersCountQuery, int>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public GetAllUsersCountHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<int> Handle(GetAllUsersCountQuery request, CancellationToken cancellationToken)
        {
            return await _userManagementRepository.GetAllUsersCountAsync();
        }
    }
}
