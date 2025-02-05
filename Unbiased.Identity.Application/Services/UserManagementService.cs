using FluentValidation;
using MediatR;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Application.Validators.User;
using Unbiased.Identity.Common.Concrete.Helpers;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.RoleManagement;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.UserManagement;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;

namespace Unbiased.Identity.Application.Services
{
    public sealed class UserManagementService : IUserManagementService
    {
        private readonly IMediator _mediator;

        public UserManagementService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            try
            {
                var result = await _mediator.Send(new GetAllUsersQuery(pageNumber, pageSize));
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<int> GetAllUsersCountAsync()
        {
            try
            {
                var result = await _mediator.Send(new GetAllUsersCountQuery());
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
        
        public async Task<bool> InsertUserWithRoles(InsertUserWithRolesDto user)
        {
            try
            {
                var validationResult = new InsertUserCustomValidation().Validate(user);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
                if (await new UserEmailAndUsernameValidation(_mediator).UserEmailAndUsernameValidationMethod(user))
                {
                    throw new ValidationException("Email or Username already exists");
                }
                user.Password = PasswordHashingExtension.ToPBKDF2Hash(user.Password);
                var result = await _mediator.Send(new InsertUserWithRolesCommand(user));
                return result;
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
        }
    }
}
