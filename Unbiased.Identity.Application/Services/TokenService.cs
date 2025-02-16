using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Unbiased.Identity.Application.Dto.Models;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Common.Concrete.Helpers;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Dto_s.Authentication;
using Unbiased.Identity.Domain.Dtos.Authentication;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IMediator _mediator;
        private readonly CustomTokenOption _customTokenOption;

        public TokenService(IMediator mediator, IOptions<CustomTokenOption> customTokenOption)
        {
            _mediator = mediator;
            _customTokenOption = customTokenOption.Value;
        }

        private string CreateRefreshToken()
        {
            var numberBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(numberBytes);
            return Convert.ToBase64String(numberBytes);
        }

        private IEnumerable<Claim> GetClaims(GetUserWithRolesDto user, List<string> audiences)
        {
            var userList = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            userList.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x)));
            return userList;
        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var clientList = new List<Claim>();
            clientList.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());
            return clientList;
        }
        public async Task<ClientTokenDto> CreateClientToken(Client client)
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

        public async Task<TokenDto> CreateToken(GetUserWithRolesDto user)
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
    }
}
