using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Dto_s.Authentication;
using Unbiased.Identity.Domain.Dtos.Authentication;

namespace Unbiased.Identity.Application.Interfaces
{
    /// <summary>
    /// Interface for token service operations providing JWT token creation functionality for users and clients.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates a JWT token for a user with their roles and permissions.
        /// </summary>
        /// <param name="user">The user with roles data transfer object containing user and role information.</param>
        /// <returns>A task that represents the asynchronous operation containing the generated token DTO.</returns>
        Task<TokenDto> CreateToken(GetUserWithRolesDto user);

        /// <summary>
        /// Creates a JWT token for client authentication.
        /// </summary>
        /// <param name="client">The client object containing client information.</param>
        /// <returns>A task that represents the asynchronous operation containing the generated client token DTO.</returns>
        Task<ClientTokenDto> CreateClientToken(Client client);
    }
}
