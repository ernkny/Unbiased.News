using MediatR;
using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement
{
    public record ValidateUsernameAndEmailWithRolesQuery(string username,string email):IRequest<bool>;
}
