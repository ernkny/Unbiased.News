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
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Identity.Application.Services
{
    /// <summary>
    /// Service implementation for user management operations providing comprehensive business logic for user management, authentication, role assignment, validation, and password hashing using CQRS pattern with MediatR.
    /// </summary>
    public sealed class UserManagementService : IUserManagementService
    {
        private readonly IMediator _mediator;
        private readonly IAuthenticationService _authenticationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the UserManagementService class.
        /// </summary>
        /// <param name="mediator">The mediator for CQRS pattern implementation.</param>
        /// <param name="authenticationService">The authentication service for token operations.</param>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        public UserManagementService(IMediator mediator, IAuthenticationService authenticationService, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _authenticationService = authenticationService;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Deletes a user and their associated roles by user identifier using CQRS command pattern with error handling and logging.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during user deletion.</exception>
        public async Task<bool> DeleteUserWithRolesAsync(int userId)
        {
            try
            {
                var result = await _mediator.Send(new DeleteUserWithRolesCommand(userId));
                return result;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Retrieves all users with pagination support using CQRS query pattern with error handling and logging.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page for pagination.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of user entities.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during users retrieval.</exception>
        public async Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            try
            {
                var result = await _mediator.Send(new GetAllUsersQuery(pageNumber, pageSize));
                return result;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }

        }

        /// <summary>
        /// Gets the total count of all users in the system using CQRS query pattern with error handling and logging.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing the total count of users.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during users count retrieval.</exception>
        public async Task<int> GetAllUsersCountAsync()
        {
            try
            {
                var result = await _mediator.Send(new GetAllUsersCountQuery());
                return result;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }

        }

        /// <summary>
        /// Retrieves a specific user with their assigned roles and permissions by user identifier using CQRS query pattern with error handling and logging.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation containing the user with roles DTO.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during user retrieval.</exception>
        public async Task<GetUserWithRolesDto> GetUserWithRolesAsync(int userId)
        {
            try
            {
                var result = await _mediator.Send(new GetUserWithRolesQuery(userId));
                return result;

            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Creates a new user with assigned roles in the system including validation, password hashing, and duplicate checking using CQRS command pattern with error handling and logging.
        /// </summary>
        /// <param name="user">The insert user with roles data transfer object containing user and role information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="ValidationException">Thrown when validation fails or email/username already exists.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during user creation.</exception>
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
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Authenticates a user with their credentials, validates password, and returns a JWT token using CQRS pattern with error handling and logging.
        /// </summary>
        /// <param name="loginDto">The login data transfer object containing user credentials.</param>
        /// <returns>A task that represents the asynchronous operation containing the authentication token DTO.</returns>
        /// <exception cref="ValidationException">Thrown when credentials are invalid or user is not found.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during authentication.</exception>
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

                var userWihtRoles = await _mediator.Send(new GetUserWithRolesQuery(userId));
                return await _authenticationService.CreateTokenAsync(userWihtRoles);
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Updates an existing user with new information and role assignments including validation using CQRS command pattern with error handling and logging.
        /// </summary>
        /// <param name="user">The update user with roles data transfer object containing updated information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="ValidationException">Thrown when validation fails.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during user update.</exception>
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
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }
    }
}
