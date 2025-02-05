using MediatR;
using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.UserManagement
{
    public record InsertUserWithRolesCommand(InsertUserWithRolesDto user):IRequest<bool>;
}
