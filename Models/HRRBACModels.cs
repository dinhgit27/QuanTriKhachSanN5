// =========================================================================
// MODULE 6: NHÂN SỰ & PHÂN QUYỀN (RBAC) - MODELS
// =========================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{


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

    // Bảng Audit_Logs: Log hệ thống (Fixed to match DB schema)
    [Table("Audit_Logs")]
    public class Audit_Log
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Column("log_date")]
        public DateTime Timestamp { get; set; }

        [Column("role_name")]
        [StringLength(255)]
        public string? RoleName { get; set; }

        [Column("log_data")]
        public string? LogData { get; set; }
    }
}
