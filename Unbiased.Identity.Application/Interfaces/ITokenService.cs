using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Dto_s.Authentication;
using Unbiased.Identity.Domain.Dtos.Authentication;

namespace Unbiased.Identity.Application.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDto> CreateToken(GetUserWithRolesDto user);
        Task<ClientTokenDto> CreateClientToken(Client client);
    }
}
