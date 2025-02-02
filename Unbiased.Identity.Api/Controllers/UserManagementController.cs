using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Application.Services;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet("/GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(int pageNumber, int pageSize)
        {
            try
            {
                var result = await _userManagementService.GetAllUsersAsync(pageNumber, pageSize);
                var response = new ResponseDto<List<User>>
                {
                    IsSuccessful = result.Any(),
                    StatusCode = result.Any() ? 200 : 204,
                    Data = result.ToList()
                };

                return result.Any() ? Ok(response) : NoContent();
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

        [HttpGet("/GetAllUsersCount")]
        public async Task<IActionResult> GetAllUsersCount()
        {
            try
            {
                var result = await _userManagementService.GetAllUsersCountAsync();
                if (result > 0)
                {
                    var response = new ResponseDto<int>
                    {
                        IsSuccessful = true,
                        StatusCode = 200,
                        Data = result
                    };
                    return Ok(response);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = "An error occurred while processing your request: " + ex.Message
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}
