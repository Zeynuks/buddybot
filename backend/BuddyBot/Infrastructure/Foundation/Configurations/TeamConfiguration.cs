using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure( EntityTypeBuilder<Team> builder )
    {
        builder.ToTable( nameof( Team ) );
        builder.HasKey( t => t.Id );

        builder.Property( t => t.Name )
               .IsRequired()
               .HasMaxLength( 100 );

        builder.HasOne( t => t.Department )
               .WithMany( d => d.Teams )
               .HasForeignKey( t => t.DepartmentId )
               .IsRequired();

        builder.HasOne( t => t.Leader )
               .WithMany()
               .HasForeignKey( t => t.LeaderId );

        builder.Property( t => t .IsDeleted )
               .IsRequired()
               .HasDefaultValue( false );

        builder.HasQueryFilter( t => !t.IsDeleted );
    }
}
