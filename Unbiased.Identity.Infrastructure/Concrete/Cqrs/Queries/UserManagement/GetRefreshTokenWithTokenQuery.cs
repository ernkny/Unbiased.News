using MediatR;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement
{
    public record GetRefreshTokenWithTokenQuery(string refreshToken):IRequest<User>;
}
