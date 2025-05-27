using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Dto_s.Authentication;
using Unbiased.Identity.Domain.Dtos.Authentication;

namespace Unbiased.Identity.Application.Interfaces
{
    /// <summary>
    /// Interface for authentication service operations providing JWT token management, refresh token handling, and client authentication functionality.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Creates a JWT token for a user with their roles and permissions.
        /// </summary>
        /// <param name="getUserWithRolesDto">The user with roles data transfer object containing user and role information.</param>
        /// <returns>A task that represents the asynchronous operation containing the generated token DTO.</returns>
        Task<TokenDto> CreateTokenAsync(GetUserWithRolesDto getUserWithRolesDto);

        /// <summary>
        /// Creates a new JWT token using a valid refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to validate and use for token generation.</param>
        /// <returns>A task that represents the asynchronous operation containing the new token DTO.</returns>
        Task<TokenDto> CreateTokenByRefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Revokes a refresh token making it invalid for future use.
        /// </summary>
        /// <param name="refreshToken">The refresh token to revoke.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RevokeRefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Creates a client-specific JWT token for client authentication.
        /// </summary>
        /// <param name="clientLoginDto">The client login data transfer object containing client credentials.</param>
        /// <returns>A task that represents the asynchronous operation containing the client token DTO.</returns>
        Task<ClientTokenDto> CreateTokenByClientAsync(ClientLoginDto clientLoginDto);
    }
}
