using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class HRInfoConfiguration : IEntityTypeConfiguration<HRInfo>
{
    public void Configure( EntityTypeBuilder<HRInfo> builder )
    {
        builder.ToTable( nameof( HRInfo ) );
        builder.HasKey( h => h.UserId );

        builder.Property( h => h.IsActive )
               .IsRequired();

        builder.HasOne( h => h.HR )
               .WithOne( u => u.HRInfo )
               .HasForeignKey<HRInfo>( h => h.UserId );
    }
}
