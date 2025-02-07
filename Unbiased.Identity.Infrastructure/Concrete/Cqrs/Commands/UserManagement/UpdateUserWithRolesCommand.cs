using MediatR;
using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.UserManagement
{
    public record UpdateUserWithRolesCommand(UpdateUserWithRolesDto user):IRequest<bool>;
}
