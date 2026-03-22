using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<User_Role> UserRoles { get; set; }
    public ICollection<Role_Permission> RolePermissions { get; set; }
}
