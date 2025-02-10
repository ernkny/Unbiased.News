using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Application.Dto.Models;
using Unbiased.Identity.Application.Interfaces;
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

        private IEnumerable<Claim> GetClaims(User user, List<string> audiences)
        {
            var userList = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return userList;
        }
        public ClientTokenDto CreateClientToken(Client client)
        {
            throw new NotImplementedException();
        }

        public TokenDto CreateToken(User user)
        {
            throw new NotImplementedException();
        }
    }
}
