using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class User
{
    [Column("status")]
<<<<<<< HEAD
    public bool IsActive { get; set; } = true; 
    
=======
    public bool IsActive { get; set; } = true;

>>>>>>> origin/dinh_nguyen
    [Column("id")]
    public int Id { get; set; }

    [Column("full_name")]
    public string FullName { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("password_hash")]
    public string PasswordHash { get; set; }

    public ICollection<User_Role> UserRoles { get; set; }
}
