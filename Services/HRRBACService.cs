// =========================================================================
// MODULE 6: HR & RBAC - SERVICE
// =========================================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Services
{
    public class HRRBACService : IHRRBACService
    {
        private readonly ApplicationDbContext _context;

        public HRRBACService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context
                .Users.Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<List<Permission>> GetPermissionsByRoleAsync(int roleId)
        {
            return await _context
                .RolePermissions.Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<bool> HasPermissionAsync(int userId, string permissionName)
        {
            var userRoles = await _context
                .UserRoles.Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();
            var permissions = await _context
                .RolePermissions.Where(rp => userRoles.Contains(rp.RoleId))
                .Select(rp => rp.Permission.Name)
                .ToListAsync();
            return permissions.Contains(permissionName);
        }

        public async Task LogActionAsync(
            int userId,
            string action,
            string tableName,
            int recordId,
            string details
        )
        {
            var user = await GetUserByIdAsync(userId);
            var roleName = user?.UserRoles?.FirstOrDefault()?.Role?.Name ?? "Unknown";

            var actionDetailObj = new {
                eventId = Guid.NewGuid().ToString("N").Substring(0, 8),
                actionType = action,
                targetTable = tableName,
                recordId = recordId,
                details = details,
                status = "Success",
                timestamp = DateTime.UtcNow
            };
            var actionDetailJson = System.Text.Json.JsonSerializer.Serialize(actionDetailObj);

            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC [dbo].[sp_InsertBatchedAuditLog] @UserId = {userId}, @RoleName = {roleName}, @ActionDetail = {actionDetailJson}"
            );
        }
    }
}
