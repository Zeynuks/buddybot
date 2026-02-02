using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure( EntityTypeBuilder<Department> builder )
    {
        builder.ToTable( nameof( Department ) );
        builder.HasKey( d => d.Id );

        builder.Property( d => d.Name )
               .IsRequired()
               .HasMaxLength( 100 );

        builder.Property( d => d.HeadFirstName )
               .HasMaxLength( 50 ); 
        
        builder.Property( d => d.HeadLastName )
               .HasMaxLength( 50 ); 

        builder.Property( d => d.HeadVideoUrl )
               .HasMaxLength( 200 );
        
        builder.Property( d => d.HeadMicrosoftTeamsUrl)
               .HasMaxLength( 200 );

        builder.Property( d => d.IsDeleted )
               .IsRequired()
               .HasDefaultValue( false );

        builder.HasQueryFilter( d => !d.IsDeleted );
    }
}
