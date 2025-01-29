using MediatR;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.RoleManagement;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Handlers.RoleManagement
{
    public record GetAllPagesWithPermissionsHandler : IRequestHandler<GetAllPagesWithPermissionsQuery, IEnumerable<PagesWithPermissionsDto>>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public GetAllPagesWithPermissionsHandler(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<IEnumerable<PagesWithPermissionsDto>> Handle(GetAllPagesWithPermissionsQuery request, CancellationToken cancellationToken)
        {
           return await _roleManagementRepository.GetAllPagesWithPermissionsAsync();
        }
    }
}
