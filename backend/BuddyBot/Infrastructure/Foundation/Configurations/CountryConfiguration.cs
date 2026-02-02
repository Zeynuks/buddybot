using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure( EntityTypeBuilder<Country> builder )
    {
        builder.ToTable( nameof( Country ) );
        builder.HasKey( c => c.Id );

        builder.Property( c => c.Name )
               .IsRequired()
               .HasMaxLength( 100 );
    }
}
