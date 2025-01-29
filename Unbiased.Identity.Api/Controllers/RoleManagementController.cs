using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unbiased.Identity.Application.Interfaces;

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
            return Ok(await _roleManagementService.GetAllPagesWithPermissionsAsync());
        }
    }
}
