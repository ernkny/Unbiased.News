using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.RoleManagement
{
    public record GetAllRolesWithoutPaginationQuery:IRequest<IEnumerable<Role>>;
}
