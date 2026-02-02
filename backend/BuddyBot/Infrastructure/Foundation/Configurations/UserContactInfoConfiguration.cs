using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using Domain.Entities;

namespace Infrastructure.Foundation.Configurations;
public class UserContactInfoConfiguration : IEntityTypeConfiguration<UserContactInfo>
{
    public void Configure( EntityTypeBuilder<UserContactInfo> builder )
    {
        builder.ToTable( nameof( UserContactInfo ) );
        builder.HasKey( uci => uci.UserId );

        builder.Property( uci => uci.FirstName )
               .HasMaxLength( 50 )
               .IsRequired();

        builder.Property( uci => uci.LastName )
               .HasMaxLength( 50 )
               .IsRequired();

        builder.Property( uci => uci.MentorVideoUrl )
               .HasMaxLength( 255 );
        
        builder.Property( uci => uci.MicrosoftTeamsUrl)
               .HasMaxLength( 255 );

        builder.Property( uci => uci.MentorPhotoUrl)
               .HasMaxLength( 255 );

        builder.Property( uci => uci.TelegramContact)
                .HasMaxLength( 100 );

        builder.Property( uci => uci.TelegramId )
                .IsRequired();

        builder.HasOne( uci => uci.User )
               .WithOne( u => u.ContactInfo )
               .HasForeignKey<UserContactInfo>( uci => uci.UserId );
    }
}
