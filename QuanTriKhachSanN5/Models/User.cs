using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuanTriKhachSanN5.Models;

public class User
{
    [Column("status")]
    public bool IsActive { get; set; } = true;

    [Column("id")]
    public int Id { get; set; }

    [Column("full_name")]
    public string FullName { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("password_hash")]
    public string PasswordHash { get; set; }

    [Column("points")]
    public int Points { get; set; } = 0;

    [Column("membership_id")]
    public int? MembershipId { get; set; }

    public Membership Membership { get; set; }

    public ICollection<User_Role> UserRoles { get; set; }
    public ICollection<User_Permission> UserPermissions { get; set; }
}
