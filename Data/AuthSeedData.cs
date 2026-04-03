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

            // Comprehensive Permissions for full RBAC
            var permissions = new List<Permission>
            {
                // Bookings
                new Permission { Name = "ViewBookings" },
                new Permission { Name = "CreateBooking" },
                new Permission { Name = "UpdateBooking" },
                new Permission { Name = "CancelBooking" },
                // Rooms
                new Permission { Name = "ViewRooms" },
                new Permission { Name = "ManageRooms" },
                new Permission { Name = "UpdateRoomStatus" },
                new Permission { Name = "ViewRoomTypes" },
                new Permission { Name = "ManageRoomTypes" },
                // Inventory
                new Permission { Name = "ViewInventory" },
                new Permission { Name = "UpdateInventory" },
                // Payments
                new Permission { Name = "ViewPayments" },
                new Permission { Name = "ManagePayments" },
                // Attractions
                new Permission { Name = "ViewAttractions" },
                new Permission { Name = "CreateAttraction" },
                new Permission { Name = "UpdateAttraction" },
                new Permission { Name = "DeleteAttraction" },
                new Permission { Name = "RestoreAttraction" },
                // CMS/Posts
                new Permission { Name = "ManagePosts" },
                new Permission { Name = "ViewPosts" },
                // Reviews
                new Permission { Name = "ViewReviews" },
                new Permission { Name = "ManageReviews" },
                // Services/Orders
                new Permission { Name = "ViewServices" },
                new Permission { Name = "ManageServices" },
                new Permission { Name = "ManageOrderServices" },
                // Loss/Damage
                new Permission { Name = "ViewLossDamages" },
                new Permission { Name = "ManageLossDamages" },
                // Users/Roles
                new Permission { Name = "ManageUsers" },
                new Permission { Name = "ManageRoles" },
                // Ops
                new Permission { Name = "CheckInOut" },
                new Permission { Name = "CleanRooms" },
                new Permission { Name = "GuestViewOnly" }
            };
            context.Permissions.AddRange(permissions);
            context.SaveChanges();

// Comprehensive Role_Permissions
            var allPermissionIds = permissions.Select(p => p.Id).ToList();
            var rolePermissions = new List<Role_Permission>();

            // Admin: ALL permissions
            foreach (var permId in allPermissionIds)
            {
                rolePermissions.Add(new Role_Permission { RoleId = roles[0].Id, PermissionId = permId });
            }

            // Receptionist: Bookings, Rooms, Attractions, Payments, Services, Inventory, CheckInOut
            var receptionistPerms = new[] { "CreateBooking", "UpdateBooking", "ViewBookings", "ManageRooms", "ViewRooms", "UpdateRoomStatus", "ViewRoomTypes", "ViewInventory", "UpdateInventory", "ViewPayments", "ManagePayments", "ViewAttractions", "CreateAttraction", "UpdateAttraction", "ManageOrderServices", "ViewServices", "ManageServices", "CheckInOut" };
            foreach (var permName in receptionistPerms)
            {
                var permId = permissions.First(p => p.Name == permName).Id;
                rolePermissions.Add(new Role_Permission { RoleId = roles[2].Id, PermissionId = permId });
            }

            // Housekeeping: Rooms, Inventory, Clean, LossDamages
            var housekeepingPerms = new[] { "ViewRooms", "UpdateRoomStatus", "ViewInventory", "UpdateInventory", "CleanRooms", "ViewLossDamages", "ManageLossDamages" };
            foreach (var permName in housekeepingPerms)
            {
                var permId = permissions.First(p => p.Name == permName).Id;
                rolePermissions.Add(new Role_Permission { RoleId = roles[3].Id, PermissionId = permId });
            }

            // Guest: limited view
            rolePermissions.Add(new Role_Permission { RoleId = roles[1].Id, PermissionId = permissions.First(p => p.Name == "GuestViewOnly").Id });

            context.RolePermissions.AddRange(rolePermissions);
            context.SaveChanges();
            context.SaveChanges();

            Console.WriteLine("✅ Auth roles, permissions seeded!");
        }
    }
}
