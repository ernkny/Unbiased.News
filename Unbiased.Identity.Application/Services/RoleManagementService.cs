using MediatR;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.RoleManagement;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.RoleManagement;

namespace Unbiased.Identity.Application.Services
{
    /// <summary>
    /// Service implementation for role management operations providing comprehensive business logic for role and permission management using CQRS pattern with MediatR.
    /// </summary>
    public sealed class RoleManagementService : IRoleManagementService
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the RoleManagementService class.
        /// </summary>
        /// <param name="mediator">The mediator for CQRS pattern implementation.</param>
        public RoleManagementService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new role with assigned permissions in the system using CQRS command pattern.
        /// </summary>
        /// <param name="role">The create role data transfer object containing role and permission information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during role creation.</exception>
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

        /// <summary>
        /// Retrieves a specific role with its assigned permissions by role identifier using CQRS query pattern.
        /// </summary>
        /// <param name="id">The unique identifier of the role to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation containing the role DTO with detailed information and permissions.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during role retrieval.</exception>
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

        /// <summary>
        /// Retrieves all pages with their associated permissions from the system using CQRS query pattern.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing a collection of pages with permissions DTOs.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during pages with permissions retrieval.</exception>
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

        /// <summary>
        /// Retrieves all roles with pagination support using CQRS query pattern.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page for pagination.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of role entities.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during roles retrieval.</exception>
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

        /// <summary>
        /// Gets the total count of all roles in the system using CQRS query pattern.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing the total count of roles.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during roles count retrieval.</exception>
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

        /// <summary>
        /// Retrieves all roles without pagination using CQRS query pattern.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing a collection of all role entities.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during roles retrieval.</exception>
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

        /// <summary>
        /// Updates an existing role with new information and permission assignments using CQRS command pattern.
        /// </summary>
        /// <param name="role">The update role data transfer object containing updated role and permission information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during role update.</exception>
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
        
        /// <summary>
        /// Deletes a role by its unique identifier using CQRS command pattern.
        /// </summary>
        /// <param name="id">The unique identifier of the role to delete.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during role deletion.</exception>
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
