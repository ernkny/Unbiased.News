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

namespace Unbiased.Identity.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediator;

        public AuthenticationService(ITokenService tokenService, IOptions<List<Client>> optionsClient, IMediator mediator)
        {
            _tokenService = tokenService;
            _clients = optionsClient.Value;
            _mediator = mediator;
        }

        public async Task<TokenDto> CreateTokenAsync(GetUserWithRolesDto getUserWithRolesDto)
        {
            try
            {
                if (getUserWithRolesDto == null) throw new ArgumentNullException(nameof(getUserWithRolesDto));
                var token= await _tokenService.CreateToken(getUserWithRolesDto);
                return token;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

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
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TokenDto> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var existRefreshToken = await _mediator.Send(new GetRefreshTokenWithTokenQuery(refreshToken));

                if (existRefreshToken == null)
                {
                   throw new Exception("Refresh token not found");
                }

                var user =  await _mediator.Send(new GetUserWithRolesQuery(existRefreshToken.UserId));

                if (user == null)
                {
                    throw new Exception("User Id not found");            }

                var tokenDto = await _tokenService.CreateToken(user);

                await _mediator.Send(new UpdateRefreshTokenByIdCommand(user.UserId, tokenDto.RefreshToken, tokenDto.RefreshTokenExpiration));
                return tokenDto;
            }
            catch (Exception)
            {

                throw;
            }
        }

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
