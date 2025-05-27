using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Dto_s.Authentication;
using Unbiased.Identity.Domain.Dtos.Authentication;
using Unbiased.Shared.Dtos.Concrete.Configurations;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Helpers;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Identity.Application.Services
{
    /// <summary>
    /// Service implementation for JWT token operations providing token creation functionality for users and clients with comprehensive error handling and logging.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly CustomTokenOption _customTokenOption;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the TokenService class.
        /// </summary>
        /// <param name="customTokenOption">The custom token configuration options.</param>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        public TokenService(IOptions<CustomTokenOption> customTokenOption, IServiceProvider serviceProvider)
        {
            _customTokenOption = customTokenOption.Value;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates a cryptographically secure refresh token using random number generation.
        /// </summary>
        /// <returns>A base64-encoded refresh token string.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during refresh token generation.</exception>
        private string CreateRefreshToken()
        {
            try
            {

                var numberBytes = new byte[32];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(numberBytes);
                return Convert.ToBase64String(numberBytes);
            }
            catch (Exception exception)
            {
                _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider).Wait();
                throw;
            }
        }

        /// <summary>
        /// Creates JWT claims for a user including user information, roles, and permissions.
        /// </summary>
        /// <param name="user">The user with roles data transfer object containing user and role information.</param>
        /// <param name="audiences">The list of audiences for the token.</param>
        /// <returns>A collection of claims for the JWT token.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during claims creation.</exception>
        private IEnumerable<Claim> GetClaims(GetUserWithRolesDto user, List<string> audiences)
        {
            try
            {
                var userList = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
                userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
                foreach (var role in user.Roles)
                {

                    userList.AddRange(role.Permissions.Select(x => new Claim("permissions", x.PermissionName)));
                }
                return userList;
            }
            catch (Exception exception)
            {
                _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider).Wait();
                throw;
            }

        }

        /// <summary>
        /// Creates JWT claims for client authentication including client-specific information.
        /// </summary>
        /// <param name="client">The client object containing client information.</param>
        /// <returns>A collection of claims for the client JWT token.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during client claims creation.</exception>
        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            try
            {
                var clientList = new List<Claim>();
                clientList.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
                new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());
                return clientList;
            }
            catch (Exception exception)
            {
                _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider).Wait();
                throw;
            }

        }

        /// <summary>
        /// Creates a JWT token for client authentication with error handling and logging.
        /// </summary>
        /// <param name="client">The client object containing client information.</param>
        /// <returns>A task that represents the asynchronous operation containing the generated client token DTO.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during client token creation.</exception>
        public async Task<ClientTokenDto> CreateClientToken(Client client)
        {
            try
            {
                var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);
                var securityKey = SigningSecurityKey.GetSymmetricSecurityKey(_customTokenOption.SecurityKey);
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                var securityToken = new JwtSecurityToken(
                    issuer: _customTokenOption.Issuer,
                    expires: accessTokenExpiration,
                    claims: GetClaimsByClient(client),
                    signingCredentials: signingCredentials
                    );
                return new ClientTokenDto()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(securityToken),
                    AccessTokenExpiration = accessTokenExpiration
                };
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }

        }

        /// <summary>
        /// Creates a JWT token for a user with their roles and permissions including access and refresh tokens with error handling and logging.
        /// </summary>
        /// <param name="user">The user with roles data transfer object containing user and role information.</param>
        /// <returns>A task that represents the asynchronous operation containing the generated token DTO.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during token creation.</exception>
        public async Task<TokenDto> CreateToken(GetUserWithRolesDto user)
        {
            try
            {
                var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);
                var securityKey = SigningSecurityKey.GetSymmetricSecurityKey(_customTokenOption.SecurityKey);
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                var securityToken = new JwtSecurityToken(
                    issuer: _customTokenOption.Issuer,
                    expires: accessTokenExpiration,
                    claims: GetClaims(user, _customTokenOption.Audience),
                    signingCredentials: signingCredentials
                    );

                var handler = new JwtSecurityTokenHandler();
                var token = handler.WriteToken(securityToken);
                var tokenDto = new TokenDto()
                {
                    AccessToken = token,
                    RefreshToken = CreateRefreshToken(),
                    AccessTokenExpiration = accessTokenExpiration,
                    RefreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.RefreshTokenExpiration)
                };
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
                }, _serviceProvider);
                throw;
            }

        }
    }
}