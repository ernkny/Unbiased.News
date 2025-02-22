using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Domain.Dto_s.Login;
using Unbiased.Identity.Domain.Dtos.Authentication;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;
using Unbiased.Shared.Extensions.Concrete.Helpers;

namespace Unbiased.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IUserManagementService userManagementService, IAuthenticationService authenticationService)
        {
            _userManagementService = userManagementService;
            _authenticationService = authenticationService;
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _userManagementService.LoginAsync(loginDto);
                var response = new ResponseDto<TokenDto>
                {
                    IsSuccessful = result is not null,
                    StatusCode = result is not null ? 200 : 204,
                    Data = result
                };

                return result is not null ? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("/refresh-token")]
        public async Task<IActionResult> RefreshAccessToken(string refreshToken)
        {
            try
            {
                var result = await _authenticationService.CreateTokenByRefreshTokenAsync(refreshToken);
                var response = new ResponseDto<TokenDto>
                {
                    IsSuccessful = result is not null,
                    StatusCode = result is not null ? 200 : 204,
                    Data = result
                };

                return result is not null ? Ok(response) : NoContent();

            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }

        }
    }
}
