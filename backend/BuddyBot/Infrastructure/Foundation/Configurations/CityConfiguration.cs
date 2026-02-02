using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure( EntityTypeBuilder<City> builder )
    {
        builder.ToTable( nameof( City ) );
        builder.HasKey( c => c.Id );

        builder.Property( c => c.Name )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.HasOne( c => c.Country )
               .WithMany( co => co.Cities )
               .HasForeignKey( c => c.CountryId )
               .IsRequired();

        builder.HasMany( c => c.Candidates )
               .WithOne( ci => ci.City )
               .HasForeignKey( ci => ci.CityId );
    }
}
