using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure( EntityTypeBuilder<Permission> builder )
    {
        builder.ToTable( nameof( Permission ) );
        builder.HasKey( p => p.PermissionName );

        builder.Property( p => p.PermissionName )
               .HasConversion<string>();

        builder.Property( p => p.Description )
               .HasMaxLength( 500 );

        builder.HasData(
            Enum.GetValues<PermissionName>()
                .Select( pn => new Permission( pn )
                {
                    Description = typeof( PermissionName )
                        .GetField( pn.ToString() )!
                        .GetCustomAttribute<DisplayAttribute>()?.Name ?? pn.ToString()
                } )
        );
    }
}
