// =========================================================================
// MODULE 6: HR & RBAC - INTERFACES
// =========================================================================

using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS_N5.API.Interfaces
{
    public interface IHRRBACService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<List<Role>> GetRolesAsync();
        Task<List<Permission>> GetPermissionsByRoleAsync(int roleId);
        Task<bool> HasPermissionAsync(int userId, string permissionName);
        Task LogActionAsync(int userId, string action, string tableName, int recordId, string details);
    }
}