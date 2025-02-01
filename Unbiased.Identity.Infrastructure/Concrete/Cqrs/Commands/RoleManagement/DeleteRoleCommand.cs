using MediatR;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.RoleManagement
{
    public record DeleteRoleCommand(int id):IRequest<bool>;
}
