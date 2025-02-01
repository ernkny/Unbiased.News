using MediatR;
using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.RoleManagement
{
    public record UpdateRoleCommand(UpdateRoleDto role) : IRequest<bool>;
}
