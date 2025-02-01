using Microsoft.AspNetCore.Mvc;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleManagementController : ControllerBase
    {
        private readonly IRoleManagementService _roleManagementService;

        public RoleManagementController(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        [HttpGet("/GetAllPagesWithPermissions")]
        public async Task<IActionResult> GetAllPagesWithPermissions()
        {
            try
            {
                var result = await _roleManagementService.GetAllPagesWithPermissionsAsync();
                var response = new ResponseDto<List<PagesWithPermissionsDto>>
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

        [HttpGet("/GetAllRoles")]
        public async Task<IActionResult> GetAllRoles(int pageNumber = 20, int pageSize = 1)
        {
            try
            {
                var result = await _roleManagementService.GetAllRolesAsync(pageNumber, pageSize);
                var response = new ResponseDto<List<Role>>
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

        [HttpGet("/GetAllRolesCount")]
        public async Task<IActionResult> GetAllRolesCount()
        {
            try
            {
                var result = await _roleManagementService.GetAllRolesCountAsync();
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

        [HttpGet("/GetRoleById")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                var result = await _roleManagementService.GetRoleByIdAsync(id);
                if (result != null)
                {
                    var response = new ResponseDto<RoleGetByIdDto>
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

        [HttpPost("/InsertRole")]
        public async Task<IActionResult> InsertRole(CreateRoleDto role)
        {
            try
            {
                var result = await _roleManagementService.CreateRoleAsync(role);
                if (result)
                {
                    var response = new ResponseDto<bool>
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
