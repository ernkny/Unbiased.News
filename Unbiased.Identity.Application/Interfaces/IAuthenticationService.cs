using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Domain.Dto_s.Authentication;
using Unbiased.Identity.Domain.Dto_s.Login;
using Unbiased.Identity.Domain.Dtos.Authentication;

namespace Unbiased.Identity.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenDto> CreateTokenAsync(LoginDto loginDto);
        Task<TokenDto> CreateTokenByRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task<ClientTokenDto> CreateTokenByClientAsync(ClientLoginDto clientLoginDto);
    }
}
