using MediatR;
using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement
{
    public record GetUserWithRolesQuery(int userId):IRequest<GetUserWithRolesDto>;
}
