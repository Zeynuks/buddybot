using Domain.Entities;
using Domain.Enums;
using Infrastructure.Foundation.Database.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure( EntityTypeBuilder<Role> builder )
    {
        builder.ToTable( nameof( Role ) );
        builder.HasKey( r => r.RoleName );

        builder.Property( r => r.RoleName )
               .HasConversion<string>();

        builder.HasData(
            Enum.GetValues<RoleName>()
                .Select( rn => new Role { RoleName = rn } )
        );

        builder.HasMany( r => r.Permissions )
            .WithMany( p => p.Roles )
            .UsingEntity<Dictionary<string, object>>(
                "PermissionRole",
                rp => rp.HasOne<Permission>().WithMany()
                        .HasForeignKey( "PermissionName" ),
                rp => rp.HasOne<Role>().WithMany()
                        .HasForeignKey( "RoleName" ),
                j => j.HasData(
                    DefaultRolePermissions.Map
                        .SelectMany( kvp => kvp.Value,
                            ( kvp, pn ) => new
                            {
                                PermissionName = pn,
                                RoleName = kvp.Key
                            } )
                )
            );
    }
}
