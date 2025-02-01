using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.RoleManagement
{
    public record GetRoleByIdQuery(int roleId):IRequest<RoleGetByIdDto>;
}
