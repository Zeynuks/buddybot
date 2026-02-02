using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class AccountCreationTokenConfiguration : IEntityTypeConfiguration<AccountCreationToken>
{
    public void Configure( EntityTypeBuilder<AccountCreationToken> builder )
    {
        builder.ToTable( nameof( AccountCreationToken ) );
        builder.HasKey( act => act.TokenValue );

        builder.Property( act => act.IssuedAtUtc )
               .IsRequired();

        builder.Property( act => act.ActivatedAtUtc );

        builder.Property( act => act.ExpireDate );

        builder.Property( act => act.Status )
               .HasConversion<string>()
               .IsRequired();

        builder.Property( act => act.TelegramInviteLink )
               .HasMaxLength( 255 );

        builder.Property( act => act.QrCodeBase64 )
               .HasColumnType( "text" );

        builder.HasOne( act => act.User )
               .WithOne()
               .HasForeignKey<AccountCreationToken>( act => act.UserId );

        builder.HasOne( act => act.Creator )
               .WithMany()
               .HasForeignKey( act => act.CreatorId );

        builder.Property( act => act.IsDeleted )
               .IsRequired()
               .HasDefaultValue( false );

        builder.HasQueryFilter( act => !act.IsDeleted );
    }
}
