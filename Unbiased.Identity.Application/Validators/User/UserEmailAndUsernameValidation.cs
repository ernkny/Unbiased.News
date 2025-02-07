using MediatR;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;

namespace Unbiased.Identity.Application.Validators.User
{
    public class UserEmailAndUsernameValidation
    {
        private readonly IMediator _mediator;

        public UserEmailAndUsernameValidation(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> UserEmailAndUsernameValidationMethod(string username,string email)
        {
            return await _mediator.Send(new ValidateUsernameAndEmailWithRolesQuery(username,email));
        }
    }
}
