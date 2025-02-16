using MediatR;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    public class GetHashedPasswordByIdHandler : IRequestHandler<GetHashedPasswordByIdQuery, string>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public GetHashedPasswordByIdHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<string> Handle(GetHashedPasswordByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userManagementRepository.GetHashedPasswordByIdAsync(request.userId);
        }
    }
}
