using MediatR;
using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.RoleManagement
{
    public record InsertRoleCommand(CreateRoleDto role) : IRequest<bool>;
}
