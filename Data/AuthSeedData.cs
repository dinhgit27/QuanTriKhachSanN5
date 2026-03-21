using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Models;
using Microsoft.EntityFrameworkCore;

namespace QuanTriKhachSanN5.Data
{
    public static class AuthSeedData
    {
        public static void SeedRolesAndPermissions(ApplicationDbContext context)
        {
            if (context.Roles.Any())
                return;

            // Roles
            var roles = new List<Role>
            {
                new Role { Name = "Admin" },
                new Role { Name = "Guest" },
                new Role { Name = "Receptionist" },
                new Role { Name = "Housekeeping" }
            };
            context.Roles.AddRange(roles);
            context.SaveChanges();

            // Permissions examples
            var permissions = new List<Permission>
            {
                new Permission { Name = "ManageBookings" },
                new Permission { Name = "ViewInventory" },
                new Permission { Name = "CheckInOut" },
                new Permission { Name = "CleanRooms" },
                new Permission { Name = "ManageUsers" }
            };
            context.Permissions.AddRange(permissions);
            context.SaveChanges();

            // Role_Permissions (Admin has all, etc.)
            var rolePermissions = new List<Role_Permission>
            {
                // Admin all
                new Role_Permission { RoleId = roles[0].Id, PermissionId = permissions[0].Id },
                new Role_Permission { RoleId = roles[0].Id, PermissionId = permissions[1].Id },
                new Role_Permission { RoleId = roles[0].Id, PermissionId = permissions[2].Id },
                new Role_Permission { RoleId = roles[0].Id, PermissionId = permissions[3].Id },
                new Role_Permission { RoleId = roles[0].Id, PermissionId = permissions[4].Id },
                // Receptionist: bookings, checkin, inventory
                new Role_Permission { RoleId = roles[2].Id, PermissionId = permissions[0].Id },
                new Role_Permission { RoleId = roles[2].Id, PermissionId = permissions[1].Id },
                new Role_Permission { RoleId = roles[2].Id, PermissionId = permissions[2].Id },
                // Housekeeping: inventory, clean
                new Role_Permission { RoleId = roles[3].Id, PermissionId = permissions[1].Id },
                new Role_Permission { RoleId = roles[3].Id, PermissionId = permissions[3].Id },
                // Guest: none
            };
            context.RolePermissions.AddRange(rolePermissions);
            context.SaveChanges();

            Console.WriteLine("✅ Auth roles, permissions seeded!");
        }
    }
}
