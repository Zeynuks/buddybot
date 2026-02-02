using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure( EntityTypeBuilder<User> builder )
    {
        builder.ToTable( nameof( User ) );
        builder.HasKey( u => u.Id );

        builder.Property( u => u.Login )
               .HasMaxLength( 100 );
        builder.HasIndex( u => u.Login )
               .IsUnique()
               .HasFilter( "\"Login\" IS NOT NULL AND \"IsDeleted\" = FALSE" );

        builder.Property( u => u.PasswordHash )
               .HasMaxLength( 255 );

        builder.Property( u => u.CreatedAtUtc )
               .IsRequired();

        builder.Property( c => c.OnboardingAccessTimeUtc );

        builder.HasMany( u => u.Roles )
               .WithMany( r => r.Users );
        
        builder.HasOne( u => u.Team )
               .WithMany( t => t.Members )
               .HasForeignKey( u => u.TeamId );

        builder.HasMany( u => u.MentoredCandidates )
               .WithMany( c => c.Mentors )
               .UsingEntity(j => j.ToTable("CandidateMentor"));

        builder.HasMany( u => u.HRCandidates )
               .WithMany( c => c.HRs )
               .UsingEntity( j => j.ToTable( "CandidateHR" ) );

        builder.Property( u => u.IsDeleted )
               .IsRequired()
               .HasDefaultValue( false );
    }
}
