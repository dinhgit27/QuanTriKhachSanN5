// =========================================================================
// MODULE 6: HR & RBAC - CONTROLLER
// =========================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class HRRBACController : ControllerBase
    {
        private readonly IHRRBACService _hrService;

        public HRRBACController(IHRRBACService hrService)
        {
            _hrService = hrService;
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _hrService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<Role>>> GetRoles()
        {
            var roles = await _hrService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpGet("roles/{roleId}/permissions")]
        public async Task<ActionResult<List<Permission>>> GetPermissions(int roleId)
        {
            var permissions = await _hrService.GetPermissionsByRoleAsync(roleId);
            return Ok(permissions);
        }

        [HttpGet("users/{userId}/permissions/{permissionName}")]
        public async Task<IActionResult> CheckPermission(int userId, string permissionName)
        {
            var hasPermission = await _hrService.HasPermissionAsync(userId, permissionName);
            return Ok(new { HasPermission = hasPermission });
        }

        [HttpPost("audit")]
        public async Task<IActionResult> LogAction([FromBody] AuditRequest request)
        {
            await _hrService.LogActionAsync(request.UserId, request.Action, request.TableName, request.RecordId, request.Details);
            return Ok("Logged");
        }
    }

    // DTO cho request
    public class AuditRequest
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public string Details { get; set; }
    }
}