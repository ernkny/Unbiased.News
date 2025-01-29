using MediatR;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.RoleManagement;

namespace Unbiased.Identity.Application.Services
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly IMediator _mediator;

        public RoleManagementService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<PagesWithPermissionsDto>> GetAllPagesWithPermissionsAsync()
        {
            return await _mediator.Send(new GetAllPagesWithPermissionsQuery());
        }
    }
}
