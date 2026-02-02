using Domain.Enums;

namespace Domain.Entities;
public class Permission
{
    public PermissionName PermissionName { get; init; }
    public string? Description { get; set; }

    public List<Role> Roles { get; private set; }

    public Permission( PermissionName permissionName )
    {
        PermissionName = permissionName;
        Roles = new List<Role>();
    }

    public Permission( PermissionName permissionName, string description )
        : this( permissionName )
    {
        Description = description;
    }
}
