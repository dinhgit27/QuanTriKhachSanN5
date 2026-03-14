// =========================================================================
// MODULE 6: HR & RBAC - SERVICE
// =========================================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KS_N5.API.Data;
using KS_N5.API.Interfaces;
using KS_N5.API.Models;

namespace KS_N5.API.Services
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
            return await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<List<Permission>> GetPermissionsByRoleAsync(int roleId)
        {
            return await _context.Role_Permissions.Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission).ToListAsync();
        }

        public async Task<bool> HasPermissionAsync(int userId, string permissionName)
        {
            var userRoles = await _context.User_Roles.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToListAsync();
            var permissions = await _context.Role_Permissions.Where(rp => userRoles.Contains(rp.RoleId))
                .Select(rp => rp.Permission.Name).ToListAsync();
            return permissions.Contains(permissionName);
        }

        public async Task LogActionAsync(int userId, string action, string tableName, int recordId, string details)
        {
            var log = new Audit_Log
            {
                UserId = userId,
                Action = action,
                TableName = tableName,
                RecordId = recordId,
                Details = details,
                Timestamp = System.DateTime.Now
            };
            _context.Audit_Logs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}