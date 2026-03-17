// =========================================================================
// MODULE 6: NHÂN SỰ & PHÂN QUYỀN (RBAC) - MODELS
// =========================================================================

namespace QuanTriKhachSanN5.Models
{
    // Bảng Users: Tài khoản người dùng
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // Guest, Receptionist, Housekeeping, Manager
        public DateTime CreatedAt { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public ICollection<User_Role> UserRoles { get; set; }
    }

    // Bảng Roles: Nhóm quyền
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } // Admin, Receptionist, etc.
        public ICollection<Role_Permission> RolePermissions { get; set; }
        public ICollection<User_Role> UserRoles { get; set; }
    }

    // Bảng Permissions: Quyền cụ thể
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; } // CreateBooking, ViewRooms, etc.
        public ICollection<Role_Permission> RolePermissions { get; set; }
    }

    // Bảng Role_Permissions: Liên kết Role và Permission
    public class Role_Permission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }

    // Bảng User_Roles: Liên kết User và Role (many-to-many)
    public class User_Role
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    // Bảng Audit_Logs: Log hệ thống
    public class Audit_Log
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Action { get; set; } // Create, Update, Delete
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
    }
}