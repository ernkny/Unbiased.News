using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.UserManagement
{
    public class GetUserRefreshTokenByIdHandler : IRequestHandler<GetUserRefreshTokenByIdQuery, string>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public GetUserRefreshTokenByIdHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<string> Handle(GetUserRefreshTokenByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userManagementRepository.GetRefreshTokenByIdAsync(request.UserId);
        }
    }
}
