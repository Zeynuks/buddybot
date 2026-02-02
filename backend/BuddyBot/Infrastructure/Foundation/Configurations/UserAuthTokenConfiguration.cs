using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class UserAuthTokenConfiguration : IEntityTypeConfiguration<UserAuthToken>
{
    public void Configure( EntityTypeBuilder<UserAuthToken> builder )
    {
        builder.ToTable( nameof( UserAuthToken ) );
        builder.HasKey( uat => uat.UserId );

        builder.Property( uat => uat.RefreshToken )
               .IsRequired()
               .HasMaxLength( 200 );

        builder.Property( uat => uat.ExpireDate )
               .IsRequired();

        builder.HasOne( uat => uat.User )
               .WithOne( u => u.AuthToken )
               .HasForeignKey<UserAuthToken>( uat => uat.UserId );
    }
}
