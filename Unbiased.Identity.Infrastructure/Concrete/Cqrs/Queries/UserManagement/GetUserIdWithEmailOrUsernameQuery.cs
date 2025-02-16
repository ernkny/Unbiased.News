using MediatR;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement
{
    public record GetUserIdWithEmailOrUsernameQuery(string EmailOrUsername):IRequest<int>;
}
