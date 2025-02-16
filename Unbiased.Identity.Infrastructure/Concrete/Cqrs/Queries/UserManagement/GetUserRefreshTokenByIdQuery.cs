using MediatR;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement
{
    public record GetUserRefreshTokenByIdQuery(int UserId):IRequest<string>;
}
