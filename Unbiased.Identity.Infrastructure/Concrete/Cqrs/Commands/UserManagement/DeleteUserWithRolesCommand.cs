using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.UserManagement
{
    public record DeleteUserWithRolesCommand(int userId) : IRequest<bool>;
}
