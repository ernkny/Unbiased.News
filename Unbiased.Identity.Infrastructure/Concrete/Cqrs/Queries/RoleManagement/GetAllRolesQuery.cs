using MediatR;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.RoleManagement
{
    public record GetAllRolesQuery(int pageNumber,int pageSize):IRequest<IEnumerable<Role>>;
}
