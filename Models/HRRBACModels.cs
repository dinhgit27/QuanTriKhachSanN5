// =========================================================================
// MODULE 6: NHÂN SỰ & PHÂN QUYỀN (RBAC) - MODELS
// =========================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{
    // Bảng Users: Tài khoản người dùng
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // Admin, User, Receptionist, Housekeeping
        public DateTime CreatedAt { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public ICollection<User_Role> UserRoles { get; set; }

        public string? Username { get; set; }

        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        public string? Role { get; set; } // Admin, User, Receptionist, Housekeeping

        [Column("status")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
        public virtual ICollection<User_Role> UserRoles { get; set; } = new List<User_Role>();
    }

    // Bảng Roles: Nhóm quyền
    [Table("Roles")]
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } // Admin, Receptionist, etc.
        public ICollection<Role_Permission> RolePermissions { get; set; }
        public ICollection<User_Role> UserRoles { get; set; }
        public string Name { get; set; } = string.Empty; // Admin, Receptionist, etc.
        public virtual ICollection<Role_Permission> RolePermissions { get; set; } = new List<Role_Permission>();
        public virtual ICollection<User_Role> UserRoles { get; set; } = new List<User_Role>();
    }

    // Bảng Permissions: Quyền cụ thể
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } // CreateBooking, ViewRooms, etc.
        public ICollection<Role_Permission> RolePermissions { get; set; }
        public string Name { get; set; } = string.Empty; // CreateBooking, ViewRooms, etc.
        public virtual ICollection<Role_Permission> RolePermissions { get; set; } = new List<Role_Permission>();
    }

    // Bảng Role_Permissions: Liên kết Role và Permission
    [Table("Role_Permissions")]
    public class Role_Permission
    {
        [Key]
        public int Id { get; set; }
        
        [Column("role_id")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public virtual Role? Role { get; set; }
        
        [Column("permission_id")]
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
        public virtual Permission? Permission { get; set; }
    }

    // Bảng User_Roles: Liên kết User và Role (many-to-many)
    [Table("User_Roles")]
    public class User_Role
    {
        [Key]
        public int Id { get; set; }
        
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }
        public virtual User? User { get; set; }
        
        [Column("role_id")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public virtual Role? Role { get; set; }
    }

    // Bảng Audit_Logs: Log hệ thống
    [Table("Audit_Logs")]
    public class Audit_Log
    {
        [Key]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }
        public string Action { get; set; } // Create, Update, Delete
        public string TableName { get; set; }
        public virtual User? User { get; set; }
        public string Action { get; set; } = string.Empty; // Create, Update, Delete
        [Column("table_name")]
        public string TableName { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string? Details { get; set; }
    }
}
