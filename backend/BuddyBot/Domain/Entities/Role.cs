using Domain.Enums;

namespace Domain.Entities;
public class Role
{
    public RoleName RoleName { get; init; }

    public List<User> Users { get; private set; }
    public List<Permission> Permissions { get; private set; }

    public Role()
    {
        Users = new List<User>();
        Permissions = new List<Permission>();
    }

    public Role( RoleName roleName )
        : this( )
    {
        RoleName = roleName;
    }
}