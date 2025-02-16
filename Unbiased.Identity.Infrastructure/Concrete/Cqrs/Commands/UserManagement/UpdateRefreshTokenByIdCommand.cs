using MediatR;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.UserManagement
{
    public record UpdateRefreshTokenByIdCommand(int userId, string refreshToken,DateTime? refreshTokenExpiration) : IRequest<bool>;
}
