using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.RoleManagement
{
    public record GetAllRolesCountQuery:IRequest<int>;
}
