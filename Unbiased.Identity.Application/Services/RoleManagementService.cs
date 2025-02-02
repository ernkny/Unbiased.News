using MediatR;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.RoleManagement;
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

        public async Task<bool> CreateRoleAsync(CreateRoleDto role)
        {
            try
            {
                return await _mediator.Send(new InsertRoleCommand(role));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<RoleGetByIdDto> GetRoleByIdAsync(int id)
        {
            try
            {
                return await _mediator.Send(new GetRoleByIdQuery(id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<PagesWithPermissionsDto>> GetAllPagesWithPermissionsAsync()
        {
            try
            {

                return await _mediator.Send(new GetAllPagesWithPermissionsQuery());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync(int pageNumber, int pageSize)
        {
            try
            {
                var result = await _mediator.Send(new GetAllRolesQuery(pageNumber, pageSize));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> GetAllRolesCountAsync()
        {
            try
            {
                var result = await _mediator.Send(new GetAllRolesCountQuery());
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetAllRolesWithoutPaginationAsync()
        {
            try
            {
                var result = await _mediator.Send(new GetAllRolesWithoutPaginationQuery());
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateRoleAsync(UpdateRoleDto role)
        {

            try
            {
                var result = await _mediator.Send(new UpdateRoleCommand(role));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public async Task<bool> DeleteRoleAsync(int id)
        {

            try
            {
                var result = await _mediator.Send(new DeleteRoleCommand(id));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
