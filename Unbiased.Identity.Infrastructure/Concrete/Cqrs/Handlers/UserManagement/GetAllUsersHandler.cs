using MediatR;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public GetAllUsersHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userManagementRepository.GetAllUsersAsync(request.pageNumber, request.pageSize);
        }
    }
}
