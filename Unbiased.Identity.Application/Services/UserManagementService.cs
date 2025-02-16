using FluentValidation;
using MediatR;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Application.Validators.User;
using Unbiased.Identity.Common.Concrete.Helpers;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Dto_s.Login;
using Unbiased.Identity.Domain.Dtos.Authentication;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Commands.UserManagement;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;

namespace Unbiased.Identity.Application.Services
{
    public sealed class UserManagementService : IUserManagementService
    {
        private readonly IMediator _mediator;
        private readonly IAuthenticationService _authenticationService;

        public UserManagementService(IMediator mediator, IAuthenticationService authenticationService)
        {
            _mediator = mediator;
            _authenticationService = authenticationService;
        }

        public async Task<bool> DeleteUserWithRolesAsync(int userId)
        {
            try
            {
                var result = await _mediator.Send(new DeleteUserWithRolesCommand(userId));
                return result;
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
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

        public async Task<GetUserWithRolesDto> GetUserWithRolesAsync(int userId)
        {
            try
            {
                var result = await _mediator.Send(new GetUserWithRolesQuery(userId));
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> InsertUserWithRolesAsync(InsertUserWithRolesDto user)
        {
            try
            {
                var validationResult = new InsertUserCustomValidation().Validate(user);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
                if (await new UserEmailAndUsernameValidation(_mediator).UserEmailAndUsernameValidationMethod(user.Username, user.Email))
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

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var userId = await _mediator.Send(new GetUserIdWithEmailOrUsernameQuery(loginDto.Email));
                if (userId == 0)
                    throw new ValidationException("Incorrect password or email or username");

                var hashedPassword = await _mediator.Send(new GetHashedPasswordByIdQuery(userId));
                if (!PasswordHashingExtension.Verify(loginDto.Password, hashedPassword))
                    throw new ValidationException("Incorrect password or email or username");

                var userWihtRoles= await _mediator.Send(new GetUserWithRolesQuery(userId));
                return await _authenticationService.CreateTokenAsync(userWihtRoles);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateUserWithRolesAsync(UpdateUserWithRolesDto user)
        {
            try
            {
                var validationResult = new UpdateUserCustomValidation().Validate(user);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
                var result = await _mediator.Send(new UpdateUserWithRolesCommand(user));
                return result;
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
        }
    }
}
