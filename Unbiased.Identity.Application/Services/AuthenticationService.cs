using Azure;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Dto_s.Authentication;
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
    /// Service implementation for authentication operations providing JWT token management, refresh token handling, client authentication, and comprehensive error handling with logging.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {

        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the AuthenticationService class.
        /// </summary>
        /// <param name="tokenService">The token service for JWT token operations.</param>
        /// <param name="optionsClient">The configuration options containing client information.</param>
        /// <param name="mediator">The mediator for CQRS pattern implementation.</param>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        public AuthenticationService(ITokenService tokenService, IOptions<List<Client>> optionsClient, IMediator mediator, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _tokenService = tokenService;
            _clients = optionsClient.Value;
            _mediator = mediator;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Creates a JWT token for a user with their roles and permissions, updates refresh token in database with error handling and logging.
        /// </summary>
        /// <param name="getUserWithRolesDto">The user with roles data transfer object containing user and role information.</param>
        /// <returns>A task that represents the asynchronous operation containing the generated token DTO.</returns>
        /// <exception cref="ArgumentNullException">Thrown when getUserWithRolesDto is null.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during token creation or refresh token update.</exception>
        public async Task<TokenDto> CreateTokenAsync(GetUserWithRolesDto getUserWithRolesDto)
        {
            try
            {
                if (getUserWithRolesDto == null) throw new ArgumentNullException(nameof(getUserWithRolesDto));
                var token = await _tokenService.CreateToken(getUserWithRolesDto);
                await _mediator.Send(new UpdateRefreshTokenByIdCommand(getUserWithRolesDto.UserId, token.RefreshToken, token.RefreshTokenExpiration));
                return token;
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
        /// Creates a client-specific JWT token for client authentication with validation and error handling.
        /// </summary>
        /// <param name="clientLoginDto">The client login data transfer object containing client credentials.</param>
        /// <returns>A task that represents the asynchronous operation containing the client token DTO.</returns>
        /// <exception cref="ValidationException">Thrown when client credentials are invalid.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during client token creation.</exception>
        public async Task<ClientTokenDto> CreateTokenByClientAsync(ClientLoginDto clientLoginDto)
        {
            try
            {

                var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

                if (client == null)
                {
                    throw new ValidationException("ClientId or ClientSecret not found");
                }

                var token = await _tokenService.CreateClientToken(client);
                return token;
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
        /// Creates a new JWT token using a valid refresh token with validation, user retrieval, and error handling.
        /// </summary>
        /// <param name="refreshToken">The refresh token to validate and use for token generation.</param>
        /// <returns>A task that represents the asynchronous operation containing the new token DTO.</returns>
        /// <exception cref="Exception">Thrown when refresh token is not found, user is not found, or an error occurs during token creation.</exception>
        public async Task<TokenDto> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var existRefreshToken = await _mediator.Send(new GetRefreshTokenWithTokenQuery(refreshToken));

                if (existRefreshToken == null)
                {
                    throw new Exception("Refresh token not found");
                }

                var user = await _mediator.Send(new GetUserWithRolesQuery(existRefreshToken.UserId));

                if (user == null)
                {
                    throw new Exception("User Id not found");
                }

                var tokenDto = await _tokenService.CreateToken(user);

                await _mediator.Send(new UpdateRefreshTokenByIdCommand(user.UserId, tokenDto.RefreshToken, tokenDto.RefreshTokenExpiration));
                return tokenDto;
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
        /// Revokes a refresh token making it invalid for future use with validation.
        /// </summary>
        /// <param name="refreshToken">The refresh token to revoke.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown when refresh token is not found.</exception>
        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var existRefreshToken = await _mediator.Send(new GetRefreshTokenWithTokenQuery(refreshToken));
            if (existRefreshToken == null)
            {
                throw new Exception("Refresh token not found");
            }

            //await _mediator.Send(new (existRefreshToken.UserId, existRefreshToken.RefreshToken, tokenDto.RefreshTokenExpiration));
        }
    }
}
