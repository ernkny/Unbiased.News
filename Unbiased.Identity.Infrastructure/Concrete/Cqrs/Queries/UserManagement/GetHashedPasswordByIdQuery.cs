using MediatR;

namespace Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement
{
    public record GetHashedPasswordByIdQuery(int userId):IRequest<string>;
}
