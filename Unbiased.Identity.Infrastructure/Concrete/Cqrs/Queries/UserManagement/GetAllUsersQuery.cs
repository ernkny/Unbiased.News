using MediatR;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement
{
    public record GetAllUsersQuery(int pageNumber, int pageSize):IRequest<IEnumerable<User>>;
}
