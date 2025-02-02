using MediatR;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement
{
    public record GetAllUsersCountQuery : IRequest<int>;
}
