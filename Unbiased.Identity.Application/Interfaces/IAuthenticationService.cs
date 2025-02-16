using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Dto_s.Authentication;
using Unbiased.Identity.Domain.Dtos.Authentication;

namespace Unbiased.Identity.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenDto> CreateTokenAsync(GetUserWithRolesDto getUserWithRolesDto);
        Task<TokenDto> CreateTokenByRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task<ClientTokenDto> CreateTokenByClientAsync(ClientLoginDto clientLoginDto);
    }
}
